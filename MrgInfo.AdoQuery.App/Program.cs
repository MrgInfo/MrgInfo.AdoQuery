using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Core.Fake;
using MrgInfo.AdoQuery.Oracle;
using MrgInfo.AdoQuery.Sql;
using PerrysNetConsole;
using static PerrysNetConsole.CoEx;

namespace MrgInfo.AdoQuery.App
{
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    static class Program
    {
        static int Count(SqlProvider provider, string? prefix) =>
            provider.Read<int>(1.LocalIdFor($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"));

        static async Task<int> CountAsync(SqlProvider provider, string? prefix) =>
            await provider.ReadAsync<int>(1.LocalIdFor($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"))
                .ConfigureAwait(false);

        static RowCollection? Load(SqlProvider provider, string? prefix)
        {
            var table = new List<string[]>();
            foreach ((int id, string code, string name, double unitprice) in
                provider.Query<int, string, string, double>(42.IdFor($@"
                    |select productid,
                    |       code,
                    |       name,
                    |       unitprice
                    |  from product
                    | where code {prefix:=*}")))
            {
                table.Add(new[] { $"{id}", $"{code}", $"{name}", $"{unitprice}" });
            }
            return RowCollection.Create(table).Import(0, RowConf.Create("Id", "Code", "Name", "Unit price").PresetTH());
        }

        static async Task<RowCollection?> LoadAsync(SqlProvider provider, string prefix, CancellationToken token = default)
        {
            var table = new List<string[]>();
            await foreach ((int id, string code, string name, double unitprice) in
                provider.QueryAsync<int, string, string, double>(42.IdFor($@"
                    |select productid,
                    |       code,
                    |       name,
                    |       unitprice
                    |  from product
                    | where code {prefix:=*}"),
                    token)
                .ConfigureAwait(false))
            {
                if (token.IsCancellationRequested) break;
                table.Add(new[] { $"{id}", $"{code}", $"{name}", $"{unitprice}" });
            }
            return RowCollection.Create(table).Import(0, RowConf.Create("Id", "Code", "Name", "Unit price").PresetTH());
        }

        static async Task RunAsync(SqlProvider provider)
        {
            WriteTitle(provider.ToString());
            WriteLine($"Count AB: {Count(provider, "AB")}");
            var indicator = new LoadIndicator();
            indicator.Start();
            RowCollection? table = Load(provider, "AB");
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
            WriteLine($"Count PQ: {await CountAsync(provider, "PQ").ConfigureAwait(false)}");
            indicator.Start();
            table = await LoadAsync(provider, "PQ").ConfigureAwait(false);
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
        }

        static FakeSqlProvider CreateFake()
        {
            IReadOnlyList<object?>[] product =
            {
                new object?[] { 10, "AB123", "Leather Sofa", 1000.0 },
                new object?[] { 20, "AB456", "Baby Chair", 200.25 },
                new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
                new object?[] { 40, "PQ123", "Sony Digital Camera", 399.0 },
                new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050.0 },
                new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
                new object?[] { 70, "PQ945", null, 150.15 },
            };
            var fakeData = new FakeIdSqlProvider
            {
                [nameof(Count), "1"] = new IReadOnlyList<object?>[] { new object?[] { product.Length } },
                [nameof(CountAsync), "1"] = new IReadOnlyList<object?>[] { new object?[] { product.Length } },
                ["42"] = product,
            };
            fakeData.Clear();
            fakeData.Register(nameof(Count), 1, new object?[] { product.Length });
            fakeData.Register(nameof(CountAsync), 1, new object?[] { product.Length });
            fakeData.Register(42, product);
            return fakeData;
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        static async Task Main()
        {
            BufferHeight = WindowHeight = WindowHeightMax - 6;
            BufferWidth = WindowWidth = WindowWidthMax - 6;
            CursorVisible = false;
            RowCollection.DefaultSettings.Border.Enabled = true;
            var microsoft = new SqlProvider(new SqlDatabaseSettings("Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;"));
            var oracle = new SqlProvider(new OracleDatabaseSettings("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=corpolis.rcinet.local)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=corpolis.rcinet.local)));User Id=adoquery;Password=adoquery;"));
            FakeSqlProvider fake = CreateFake();
            await RunAsync(microsoft).ConfigureAwait(false);
            await RunAsync(oracle).ConfigureAwait(false);
            await RunAsync(fake).ConfigureAwait(false);
            fake.SaveAllQueries("queries.xml");
            WriteHl("Program finished.");
        }
    }
}
