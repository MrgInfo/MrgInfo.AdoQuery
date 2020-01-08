using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Core.FormattableStrings;
using MrgInfo.AdoQuery.Oracle;
using MrgInfo.AdoQuery.Sql;
using Xunit;
using Xunit.Abstractions;

namespace MrgInfo.AdoQuery.Test
{
    /// <summary>
    ///     Adatbázis lekérdezéséket futtató szolgáltatás tesztesetek.
    /// </summary>
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    public sealed class SqlProviderTests
    {
        ITestOutputHelper Output { get; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="output">
        ///     Kimenetkezelő.
        /// </param>
        public SqlProviderTests(ITestOutputHelper output) =>
            Output = output ?? throw new ArgumentNullException(nameof(output));

        /// <summary>
        ///     Adatbázis-kiszolgálók.
        /// </summary>
        /// <returns>
        ///     Adatbázis elérés lista.
        /// </returns>
        public static IEnumerable<object[]> Vendors
        {
            get
            {
                yield return new object[] { new SqlDatabaseSettings("Data Source=testmssql;User Id=poszeidon_teszt;Password=Passw0rd;Initial Catalog=Poszeidon_teszt_uj;") };
                yield return new object[] { new OracleDatabaseSettings("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=innerora)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=innerora)));User Id=MEDIATOR_4;Password=POSZEIDON;") };
            }
        }

        /// <summary>
        ///     Adatbázis elérések tesztelése.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestConnection(IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            var watch = Stopwatch.StartNew();

            var cnt = provider.Read<int>(1.Is($@"
                |select count(*)
                |  from t_irat
                | where c_keletkezes > {DateTime.Today.AddDays(-40)}"));
            
            watch.Stop();

            Assert.InRange(cnt, 0, 100_000);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     A projekcióból kimaradó oszlopok alapértelmezett értékkel töltődnek.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [SuppressMessage("ReSharper", "UseDeconstructionOnParameter")]
        [Theory, MemberData(nameof(Vendors))]
        public void TestExtraColumns(IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            var watch = Stopwatch.StartNew();

            (int Id, string Iktatoszam, int? Nullable, double Float)[] result =
                provider.Query<int, string, int?, double>($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat
                    | where c_keletkezes > {DateTime.Today.AddDays(-40)}")
                .ToArray();

            watch.Stop();

            Assert.All(result, row =>
            {
                Assert.Null(row.Nullable);
                Assert.Equal(0.0, row.Float);
            });

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Adatbázis <c>NULL</c> tesztelése.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestNull(IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            var watch = Stopwatch.StartNew();

            Assert.All(
                provider.Query<int?, string>($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat
                    | where c_szovegesisz is null
                    |   and c_keletkezes > {DateTime.Today.AddDays(-40)}"),
                row =>
                {
                    (var column1, string column2) = row;
                    Assert.NotNull(column1);
                    Assert.Null(column2);
                });

            watch.Stop();

            Output.WriteLine($"{watch.Elapsed:g}");
        }
    }
}
