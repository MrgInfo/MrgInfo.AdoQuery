using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MrgInfo.AdoQuery.Core;
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
    public sealed class DbQueryProviderTests
    {
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
                yield return new object[] { new SqlQueryProvider("Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;") };
                // yield return new object[] { new SqlQueryProvider("Data Source=.;Integrated Security=True;Initial Catalog=AdoQuery;") };
                yield return new object[] { new OracleQueryProvider("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=corpolis.rcinet.local)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=corpolis.rcinet.local)));User Id=adoquery;Password=adoquery;") };
                // yield return new object[] { new OracleDatabaseSettings("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=/;Password=;Integrated Security=True") };
            }
        }

        ITestOutputHelper Output { get; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="output">
        ///     Kimenetkezelő.
        /// </param>
        public DbQueryProviderTests(ITestOutputHelper output) =>
            Output = output ?? throw new ArgumentNullException(nameof(output));

        /// <summary>
        ///     Adatbázis elérések tesztelése.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestConnection(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var watch = Stopwatch.StartNew();

            var cnt = provider.Read<int>(1.IdFor($@"
                |select count(*)
                |  from product
                | where unitprice > {500}"));
            
            watch.Stop();

            Assert.Equal(3, cnt);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     A projekcióból kimaradó oszlopok alapértelmezett értékkel töltődnek.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [SuppressMessage("ReSharper", "UseDeconstructionOnParameter")]
        [Theory, MemberData(nameof(Vendors))]
        public void TestExtraColumns(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var watch = Stopwatch.StartNew();

            (int ProductId, string Code, int? Nullable, double Float)[] result =
                provider.Query<int, string, int?, double>($@"
                    |  select productid,
                    |         code
                    |    from product
                    |   where unitprice > {500}
                    |order by productid")
                .ToArray();

            watch.Stop();

            Assert.All(result, row =>
            {
                Assert.Null(row.Nullable);
                Assert.Equal(default, row.Float);
            });

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Adatbázis <c>NULL</c> tesztelése.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestNull(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var watch = Stopwatch.StartNew();

            Assert.All(
                provider.Query<int?, string>($@"
                    |select productid,
                    |       name
                    |  from product
                    | where name is null
                    |   and code {"PQ":=*}"),
                row =>
                {
                    (int? column1, string column2) = row;
                    Assert.NotNull(column1);
                    Assert.Null(column2);
                });

            watch.Stop();

            Output.WriteLine($"{watch.Elapsed:g}");
        }
    }
}
