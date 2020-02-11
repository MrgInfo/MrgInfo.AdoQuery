using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Oracle;
using MrgInfo.AdoQuery.Sql;
using PerrysNetConsole;
using static PerrysNetConsole.CoEx;

namespace MrgInfo.AdoQuery.App
{
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    static class AdoConsole
    {
        static int Count(QueryProvider provider, string? prefix) =>
            provider.Read<int>(1.LocalIdFor($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"));

        static async Task<int> CountAsync(QueryProvider provider, string? prefix) =>
            await provider.ReadAsync<int>(1.LocalIdFor($@"
                |select count(productid)
                |  from product
                | where code {prefix:=*}"))
                .ConfigureAwait(false);

        static RowCollection? Load(QueryProvider provider, string? prefix)
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

        static async Task<RowCollection?> LoadAsync(QueryProvider provider, string prefix, CancellationToken token = default)
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

        static async Task RunAsync(QueryProvider provider, CancellationToken cancellationToken = default)
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
            table = await LoadAsync(provider, "PQ", cancellationToken).ConfigureAwait(false);
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
        }

        static MockQueryProvider CreateMockedData()
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
            var mock = new MockByIdQueryProvider
            {
                [nameof(Count), "1"] = new IReadOnlyList<object?>[] { new object?[] { product.Length } },
                [nameof(CountAsync), "1"] = new IReadOnlyList<object?>[] { new object?[] { product.Length } },
                ["42"] = product,
            };
            mock.Clear();
            mock.Register(nameof(Count), 1, new object?[] { product.Length });
            mock.Register(nameof(CountAsync), 1, new object?[] { product.Length });
            mock.Register(42, product);
            return mock;
        }

        static void RunMockedOnDb(MockQueryProvider mock, DbQueryProvider db)
        {
            const string fileName = "queries.xml";
            mock.SaveQueries(fileName);
            var queries = QueriesCollection.Load(fileName);
            Query.IdMapper = i => i;
            foreach (Query query in queries)
            {
                WriteLine($"Record count of {query.Id} is {query.RunCount(db)}");
            }
        }

        public static async Task RunAsync(CancellationToken cancellationToken = default)
        {
            BufferHeight = WindowHeight = WindowHeightMax - 6;
            BufferWidth = WindowWidth = WindowWidthMax - 6;
            CursorVisible = false;
            RowCollection.DefaultSettings.Border.Enabled = true;
            var microsoft = new SqlQueryProvider("Data Source=localhost;User Id=AdoQuery;Password=AdoQuery;");
            var oracle = new OracleQueryProvider("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=ORCLCDB)));User Id=adoquery;Password=adoquery;");
            MockQueryProvider mock = CreateMockedData();
            await RunAsync(microsoft, cancellationToken).ConfigureAwait(false);
            await RunAsync(oracle, cancellationToken).ConfigureAwait(false);
            await RunAsync(mock, cancellationToken).ConfigureAwait(false);
            RunMockedOnDb(mock, microsoft);
        }
    }
}
