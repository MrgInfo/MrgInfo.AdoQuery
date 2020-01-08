using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Core.Fake;
using MrgInfo.AdoQuery.Core.FormattableStrings;
using Xunit;
using Xunit.Abstractions;

namespace MrgInfo.AdoQuery.Test
{
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    public sealed class FakeSqlProviderTests
    {
        ITestOutputHelper Output { get; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="output">
        ///     Kimenetkezelő.
        /// </param>
        public FakeSqlProviderTests(ITestOutputHelper output) =>
            Output = output ?? throw new ArgumentNullException(nameof(output));

        /// <summary>
        ///     Hamisítás tesztelése egyedi <see cref="Guid"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithGuid()
        {
            Guid query1 = Guid.NewGuid(), query2 = Guid.NewGuid();
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

            var watch = Stopwatch.StartNew();

            (int id0, string iktatoszam0) =
                provider
                    .Query<int, string>(query1.Is($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat"))
                    .FirstOrDefault();
            (int id1, string iktatoszam1) =
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

            var watch = Stopwatch.StartNew();

            (int id0, string iktatoszam0) =
                provider
                    .Query<int, string>(1.Is($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat"))
                    .FirstOrDefault();
            (int id1, string iktatoszam1) =
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

            var watch = Stopwatch.StartNew();

            (int id0, string iktatoszam0) =
                provider
                    .Query<int, string>($@"
                    |select id,
                    |       c_szovegesisz
                    |  from t_irat")
                    .FirstOrDefault();
            (int id1, string iktatoszam1) =
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
        [Fact]
        public void TestFakeSerialize()
        {
            const int numb = 100000;
            const string str = "x";

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

            using var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml));
            using var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            var serializer = new DataContractSerializer(typeof(SqlQueriesCollection));
            var queries = serializer.ReadObject(reader, false) as SqlQueriesCollection;
            
            Assert.NotNull(queries);
            Assert.NotEmpty(queries);
            Assert.NotNull(queries![0]);

            Output.WriteLine($"{watch.Elapsed:g}");
        }
    }
}