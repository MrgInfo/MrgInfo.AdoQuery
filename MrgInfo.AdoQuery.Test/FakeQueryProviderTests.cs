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
using Xunit;
using Xunit.Abstractions;

namespace MrgInfo.AdoQuery.Test
{
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    public sealed class FakeQueryProviderTests
    {
        ITestOutputHelper Output { get; }

        /// <summary>
        ///     TODO Konstruktor.
        /// </summary>
        /// <param name="output">
        ///     TODO Kimenetkezelő.
        /// </param>
        public FakeQueryProviderTests(ITestOutputHelper output) =>
            Output = output ?? throw new ArgumentNullException(nameof(output));

        static IList<IReadOnlyList<object?>> Product { get; } = new IReadOnlyList<object?>[]
        {
            new object?[] { 10, "AB123", "Leather Sofa", 1000.0 },
            new object?[] { 20, "AB456", "Baby Chair", 200.25 },
            new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
            new object?[] { 40, "PQ123", "Sony Digital Camera", 399 },
            new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050.0 },
            new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
            new object?[] { 70, "PQ945", null, 150.15 },
        };

        /// <summary>
        ///     TODO Hamisítás tesztelése egyedi <see cref="Guid"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithGuid()
        {
            Guid query1 = Guid.NewGuid(), query2 = Guid.NewGuid();
            var provider = new ByIdFakeQueryProvider
            {
                [$"{query1}"] = Product,
                [$"{query2}"] = Product,
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>(query1.IdFor($@"
                    |select productid,
                    |       code
                    |  from product"))
                .First();
            (int id1, string code1) = provider
                .Query<int, string>(query2.IdFor($"select productid, code from product"))
                .Last();

            watch.Stop();

            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     TODO Hamisítás tesztelése egyedi <see cref="int"/> azonosítóval.
        /// </summary>
        [Fact]
        public void TestFakeWithInt()
        {
            var provider = new ByIdFakeQueryProvider
            {
                [nameof(TestFakeWithInt), "1"] = Product,
                [nameof(TestFakeWithInt), "2"] = Product,
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>(1.LocalIdFor($@"
                    |select productid,
                    |       code
                    |  from product"))
                .First();
            (int id1, string code1) = provider
                .Query<int, string>(2.LocalIdFor($"select productid, code from product"))
                .Last();

            watch.Stop();

            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     TODO Hamisítás tesztelése reguláris kifejezéssel.
        /// </summary>
        [Fact]
        public void TestFakeWithRegex()
        {
            var provider = new ByPatternFakeQueryProvider
            {
                ["productid.+code.+product"] = Product
            };

            var watch = Stopwatch.StartNew();

            (int id0, string code0) = provider
                .Query<int, string>($@"
                    |select productid,
                    |       code
                    |  from product")
                .First();
            (int id1, string code1) = provider
                .Query<int, string>($"select productid, code from product")
                .Last();

            watch.Stop();

            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);

            Output.WriteLine($"{watch.Elapsed:g}");
        }

        /// <summary>
        ///     TODO Lekérdezés sorosítás tesztelése.
        /// </summary>
        [Fact]
        public void TestFakeSerialize()
        {
            const int numb = 100000;
            const string str = "x";

            var fakeProvider = new ByPatternFakeQueryProvider();
            var watch = new Stopwatch();
            string xml;
            using (var writer = new StringWriter())
            {
                watch.Start();
                fakeProvider.Read<DateTime?>(1.IdFor($@"
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
            var serializer = new DataContractSerializer(typeof(QueriesCollection));
            var queries = serializer.ReadObject(reader, false) as QueriesCollection;

            Assert.NotNull(queries);
            Assert.NotEmpty(queries);
            Assert.NotNull(queries![0]);

            Output.WriteLine($"{watch.Elapsed:g}");
        }
    }
}