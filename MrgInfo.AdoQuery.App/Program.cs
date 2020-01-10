using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Core.Fake;
using MrgInfo.AdoQuery.Core.FormattableStrings;
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
            provider.Read<int>(1.Is($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"));

        static async Task<int> CountAsync(SqlProvider provider, string? prefix) =>
            await provider.ReadAsync<int>(1.Is($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"))
                .ConfigureAwait(false);

        static RowCollection? Load(SqlProvider provider, string? prefix)
        {
            var table = new List<string[]>();
            foreach ((int id, string login, string name, string taj, bool outsider) in
                provider.Query<int, string, string, string, bool>("Product".Is($@"
                    |select productid,
                    |       code,
                    |       name,
                    |       unitprice
                    |  from product
                    | where code {prefix:=*}")))
            {
                table.Add(new[] { $"{id}", $"{login}", $"{name}", $"{taj}", $"{outsider}" });
            }
            var result = RowCollection.Create(table);
            result?.Import(0, RowConf.Create("Id", "Code", "Name", "Unit price").PresetTH());
            return result;
        }

        static async Task<RowCollection?> LoadAsync(SqlProvider provider, string prefix, CancellationToken token = default)
        {
            var table = new List<string[]>();
            await foreach ((int id, string login, string name, string taj, bool outsider) in 
                provider.QueryAsync<int, string, string, string, bool>("Product".Is($@"
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
                table.Add(new[] { $"{id}", login ?? "", name ?? "", taj ?? "", $"{outsider}" });
            }
            var result = RowCollection.Create(table);
            result?.Import(0, RowConf.Create("Id", "Code", "Name", "Unit price").PresetTH());
            return result;
        }

        static async Task RunAsync(SqlProvider provider)
        {
            WriteTitle(provider.ToString());
            WriteLine($"Count: {Count(provider, "AB")}");
            var indicator = new LoadIndicator();
            indicator.Start();
            RowCollection? table = Load(provider, "AB");
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
            WriteLine($"Count: {await CountAsync(provider, "PQ").ConfigureAwait(false)}");
            indicator.Start();
            table = await LoadAsync(provider, "PQ").ConfigureAwait(false);
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
        }

        static FakeSqlProvider CreateFake()
        {
            var product = new[]
            {
                new object?[] { 10, "AB123", "Leather Sofa", 1000 },
                new object?[] { 20, "AB456", "Baby Chair", 200.25 },
                new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
                new object?[] { 40, "PQ123", "Sony Digital Camera", 399 },
                new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050 },
                new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
                new object?[] { 70, "PQ945", null, 150.15 },
            };
            return new FakeIdSqlProvider
            {
                ["Product"] = product,
                [nameof(Count), 1] = product.Length.ToData(),
                [nameof(CountAsync), 1] = product.Length.ToData(),
            };
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        static async Task Main()
        {
            BufferHeight = WindowHeight = WindowHeightMax - 6;
            BufferWidth = WindowWidth = WindowWidthMax - 6;
            CursorVisible = false;
            RowCollection.DefaultSettings.Border.Enabled = true;
            await RunAsync(new SqlProvider(new SqlDatabaseSettings("Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;"))).ConfigureAwait(false);
            await RunAsync(new SqlProvider(new OracleDatabaseSettings("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=corpolis.rcinet.local)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=corpolis.rcinet.local)));User Id=adoquery;Password=adoquery;"))).ConfigureAwait(false);
            FakeSqlProvider provider = CreateFake();
            await RunAsync(provider).ConfigureAwait(false);
            provider.SaveAllQueries("queries.xml");
            WriteHl("Program finished.");
            Confirm("Exit?");
        }
    }
}
