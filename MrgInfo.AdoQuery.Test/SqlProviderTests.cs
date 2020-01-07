using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Devart.Data.Oracle;
using Xunit;
using Xunit.Abstractions;
using static System.Guid;
using static Sda.Query.DatabaseSettingsFactory;

namespace Sda.Query.Tests
{
    /// <summary>
    ///     Adatbázis lekérdezéséket futtató szolgáltatás tesztesetek.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    public sealed class SqlProviderTests
    {
        [NotNull]
        ITestOutputHelper Output { get; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="output">
        ///     Kimenetkezelő.
        /// </param>
        public SqlProviderTests([NotNull] ITestOutputHelper output) =>
            Output = output ?? throw new ArgumentNullException(nameof(output));

        /// <summary>
        ///     Adatbázis-kiszolgálók.
        /// </summary>
        /// <returns>
        ///     Adatbázis elérés lista.
        /// </returns>
        [NotNull, ItemNotNull]
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public static IEnumerable<object[]> Vendors
        {
            get
            {
                yield return new[] { CreateSettings("Microsoft") };
                yield return new[] { CreateSettings("Oracle") };
                yield return new[] { CreateSettings("Devart") };
                yield return new[] { CreateSettings("Hana") };
            }
        }

        /// <summary>
        ///     Hamisítás tesztelése egyedi <see cref="Guid"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithGuid()
        {
            Guid query1 = NewGuid(), query2 = NewGuid();
            var data = new[]
            {
                new object[] { 1, "A/1-2/2013" },
                new object[] { 2, "B/42/2017" }
            };
            var provider = new FakeIdSqlProvider
            {
                [query1] = data,
                [query2] = data,
            };
            Stopwatch watch = Stopwatch.StartNew();
            var (id0, iktatoszam0) =
                provider
                .Query<int, string>(query1.Is($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat"))
                .FirstOrDefault();

            var (id1, iktatoszam1) =
                provider
                .Query<int, string>(query2.Is($"select id, c_szovegesisz from t_irat"))
                .LastOrDefault();
            watch.Stop();

            Assert.Equal(data[0]?[0], id0);
            Assert.Equal(data[0]?[1], iktatoszam0);
            Assert.Equal(data[1]?[0], id1);
            Assert.Equal(data[1]?[1], iktatoszam1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Hamisítás tesztelése egyedi <see cref="int"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithInt()
        {
            var data = new[]
            {
                new object[] { 1, "A/1-2/2013" },
                new object[] { 2, "B/42/2017" }
            };
            var provider = new FakeIdSqlProvider
            {
                [nameof(TestFakeWithInt), 1] = data,
                [nameof(TestFakeWithInt), 2] = data,
            };
            Stopwatch watch = Stopwatch.StartNew();
            var (id0, iktatoszam0) =
                provider
                .Query<int, string>(1.Is($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat"))
                .FirstOrDefault();

            var (id1, iktatoszam1) =
                provider
                .Query<int, string>(2.Is($"select id, c_szovegesisz from t_irat"))
                .LastOrDefault();
            watch.Stop();

            Assert.Equal(data[0]?[0], id0);
            Assert.Equal(data[0]?[1], iktatoszam0);
            Assert.Equal(data[1]?[0], id1);
            Assert.Equal(data[1]?[1], iktatoszam1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Hamisítás tesztelése reguláris kifejezéssel.
        /// </summary>
        [Fact]
        public void TestFakeWithRegex()
        {
            var data = new[]
            {
                new object[] { 1, "A/1-2/2013" },
                new object[] { 2, "B/42/2017" }
            };
            var provider = new FakePatternSqlProvider
            {
                [@"id.+c_szovegesisz.+t_irat"] = data
            };
            Stopwatch watch = Stopwatch.StartNew();
            var (id0, iktatoszam0) =
                provider
                .Query<int, string>($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat")
                .FirstOrDefault();

            var (id1, iktatoszam1) =
                provider
                .Query<int, string>($"select id, c_szovegesisz from t_irat")
                .LastOrDefault();
            watch.Stop();

            Assert.Equal(data[0]?[0], id0);
            Assert.Equal(data[0]?[1], iktatoszam0);
            Assert.Equal(data[1]?[0], id1);
            Assert.Equal(data[1]?[1], iktatoszam1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Lekérdezés sorosítás tesztelése.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestFakeSerialize([NotNull] IDatabaseSettings settings)
        {
            const int numb = 100000;
            const string str = "x";

            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));
            var fakeProvider = new FakePatternSqlProvider();
            var watch = new Stopwatch();
            string xml;

            using (var writer = new StringWriter())
            {
                watch.Start();
                fakeProvider.Read<DateTime?>(1.Is($@"
                    |select id
                    |  from t_irat
                    | where 'xxx' {str:=*}
                    |   and 42 {null:!=}
                    |   and id > {numb}"));
                fakeProvider.SaveAllQueries(writer);
                fakeProvider.Clear();
                watch.Stop();
                xml = writer.ToString();
            }

            Assert.NotNull(xml);
            Assert.NotStrictEqual("", xml);

            Output.WriteLine(xml);

            SqlQueriesCollection queries;
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(SqlQueriesCollection));
                queries = serializer.ReadObject(reader, false) as SqlQueriesCollection;
            }

            Assert.NotNull(queries);
            Assert.NotEmpty(queries);
            Assert.NotNull(queries[0]);

            watch.Start();
            int? count = queries[0].RunCount(provider);
            watch.Stop();

            Assert.NotNull(count);
            Assert.InRange(count.Value, 0, 1_000_000);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Adatbázis elérések tesztelése.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestConnection([NotNull] IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            Stopwatch watch = Stopwatch.StartNew();
            var cnt = provider.Read<int>(1.Is($@"
                |select count(*)
                |  from t_irat
                | where c_keletkezes > {DateTime.Today.AddDays(-40)}"));

            Assert.InRange(cnt, 0, 100_000);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     A projekcióból kimaradó oszlopok alapértelmezett értékkel töltődnek.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestExtraColumns([NotNull] IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            Stopwatch watch = Stopwatch.StartNew();
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
        public void TestNull([NotNull] IDatabaseSettings settings)
        {
            var provider = new SqlProvider(settings ?? throw new ArgumentNullException(nameof(settings)));

            Stopwatch watch = Stopwatch.StartNew();
            Assert.All(
                provider.Query<int?, string>($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat
                    | where c_szovegesisz is null
                    |   and c_keletkezes > {DateTime.Today.AddDays(-40)}"),
                row =>
                {
                    Assert.NotNull(row.Column1);
                    Assert.Null(row.Column2);
                });
            watch.Stop();

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Devart kapcsolati leíró tesztelése.
        /// </summary>
        [Fact]
        public void TestDevartBuilder()
        {
            var provider = new SqlProvider(new DevartDatabaseSettings(new OracleConnectionStringBuilder
            {
                Direct = true,
                UserId = @"MEDIATOR_4",
                Password = @"POSZEIDON",
                Server = @"innerora",
                Port = 1521,
                ServiceName = @"innerora"
            }));

            Stopwatch watch = Stopwatch.StartNew();
            var now = provider.Read<DateTime>($"select sysdate from dual");
            watch.Stop();

            Assert.InRange(now, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            Output.WriteLine($"{watch.Elapsed:g}");
        }
    }
}
