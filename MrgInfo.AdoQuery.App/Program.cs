using System;
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
        static readonly RowConf _header = RowConf.Create("Id", "Login", "User", "Medical number", "Outsider?").PresetTH();

        static int Count(SqlProvider provider, string? prefix)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            return provider.Read<int>(1.Is($@"
                |select count(id)
                |  from t_user
                | where c_loginname {prefix:=*}"));
        }

        static async Task<int> CountAsync(SqlProvider provider, string? prefix)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            return await
                provider.ReadAsync<int>(1.Is($@"
                    |select count(id)
                    |  from t_user
                    | where c_loginname {prefix:=*}"))
                .ConfigureAwait(false);
        }

        static RowCollection? Load(SqlProvider provider, string? prefix)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var table = new List<string[]>();
            foreach ((int id, string login, string name, string taj, bool outsider) in
                provider.Query<int, string, string, string, bool>("Users".Is($@"
                   |select id,
                   |       c_loginname,
                   |       c_searchname,
                   |       c_tajnumber,
                   |       c_outsider
                   |  from t_user
                   | where c_loginname {prefix:=*}")))
            {
                table.Add(new[] { $"{id}", $"{login}", $"{name}", $"{taj}", $"{outsider}" });
            }
            var result = RowCollection.Create(table);
            result?.Import(0, _header);
            return result;
        }

        static async Task<RowCollection?> LoadAsync(SqlProvider provider, string prefix, CancellationToken token = default)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var table = new List<string[]>();
            foreach ((int id, string login, string name, string taj, bool outsider) in
                await provider.QueryAsync<int, string, string, string, bool>("Users".Is($@"
                    |select id,
                    |       c_loginname,
                    |       c_searchname,
                    |       c_tajnumber,
                    |       c_outsider
                    |  from t_user
                    | where c_loginname {prefix:=*}"),
                    token)
                .ConfigureAwait(false))
            {
                if (token.IsCancellationRequested) break;
                table.Add(new[] { $"{id}", login ?? "", name ?? "", taj ?? "", $"{outsider}" });
            }
            var result = RowCollection.Create(table);
            result?.Import(0, _header);
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
            WriteLine($"Count: {await CountAsync(provider, "BE").ConfigureAwait(false)}");
            indicator.Start();
            table = await LoadAsync(provider, "BE").ConfigureAwait(false);
            indicator.Stop();
            WriteTable(table);
            PressAnyKey();
        }

        static FakeSqlProvider CreateFake()
        {
            var users = new[]
            {
                new object[] { 42, "joe", "John Doe", "76543579", false },
                new object[] { 43, "wavezone", "Groma István", "87856567", true },
            };
            return new FakeIdSqlProvider
            {
                ["Users"] = users,
                [nameof(Count), 1] = users.Length.ToData(),
                [nameof(CountAsync), 1] = users.Length.ToData(),
            };
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        static async Task Main()
        {
            BufferHeight = WindowHeight = WindowHeightMax - 6;
            BufferWidth = WindowWidth = WindowWidthMax - 6;
            CursorVisible = false;
            RowCollection.DefaultSettings.Border.Enabled = true;

            await RunAsync(new SqlProvider(new SqlDatabaseSettings("Data Source=testmssql;User Id=poszeidon_teszt;Password=Passw0rd;Initial Catalog=Poszeidon_teszt_uj;"))).ConfigureAwait(false);
            await RunAsync(new SqlProvider(new OracleDatabaseSettings("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=innerora)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=innerora)));User Id=MEDIATOR_4;Password=POSZEIDON;"))).ConfigureAwait(false);
            var provider = CreateFake();
            await RunAsync(provider).ConfigureAwait(false);
            provider.SaveAllQueries("queries.xml");

            WriteHl("Program finished.");
            Confirm("Exit?");
        }
    }

}
