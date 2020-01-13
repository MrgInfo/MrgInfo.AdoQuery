using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Core.Fake;
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

        static IList<IList<object?>> Product { get; } = new List<IList<object?>>
        {
            new object?[] { 10, "AB123", "Leather Sofa", 1000 },
            new object?[] { 20, "AB456", "Baby Chair", 200.25 },
            new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
            new object?[] { 40, "PQ123", "Sony Digital Camera", 399 },
            new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050 },
            new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
            new object?[] { 70, "PQ945", null, 150.15 },
        };

        /// <summary>
        ///     Hamisítás tesztelése egyedi <see cref="Guid"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithGuid()
        {
            Guid query1 = Guid.NewGuid(), query2 = Guid.NewGuid();
            var provider = new FakeIdSqlProvider
            {
                [query1] = Product,
                [query2] = Product,
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>(query1.Is($@"
                    |select productid,
                    |       code
                    |  from product"))
                .FirstOrDefault();
            (int id1, string code1) = provider
                .Query<int, string>(query2.Is($"select productid, code from product"))
                .LastOrDefault();

            watch.Stop();

            Assert.Equal(Product[0]?[0], id0);
            Assert.Equal(Product[0]?[1], code0);
            Assert.Equal(Product[1]?[0], id1);
            Assert.Equal(Product[1]?[1], code1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Hamisítás tesztelése egyedi <see cref="int"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithInt()
        {
            var provider = new FakeIdSqlProvider
            {
                [nameof(TestFakeWithInt), 1] = Product,
                [nameof(TestFakeWithInt), 2] = Product,
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>(1.Is($@"
                    |select productid,
                    |       code
                    |  from product"))
                .FirstOrDefault();
            (int id1, string code1) = provider
                .Query<int, string>(2.Is($"select productid, code from product"))
                .LastOrDefault();

            watch.Stop();

            Assert.Equal(Product[0]?[0], id0);
            Assert.Equal(Product[0]?[1], code0);
            Assert.Equal(Product[1]?[0], id1);
            Assert.Equal(Product[1]?[1], code1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     Hamisítás tesztelése reguláris kifejezéssel.
        /// </summary>
        [Fact]
        public void TestFakeWithRegex()
        {
            var provider = new FakePatternSqlProvider
            {
                [@"productid.+code.+product"] = Product
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>($@"
                    |select productid,
                    |       code
                    |  from product")
                .FirstOrDefault();
            (int id1, string code1) = provider
                .Query<int, string>($"select productid, code from product")
                .LastOrDefault();

            watch.Stop();

            Assert.Equal(Product[0]?[0], id0);
            Assert.Equal(Product[0]?[1], code0);
            Assert.Equal(Product[1]?[0], id1);
            Assert.Equal(Product[1]?[1], code1);

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
                    |select productid
                    |  from product
                    | where 'xxx' {str:=*}
                    |   and 42 {null:!=}
                    |   and productid > {numb}"));
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