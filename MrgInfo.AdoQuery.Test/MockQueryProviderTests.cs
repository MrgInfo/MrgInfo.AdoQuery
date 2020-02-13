using System;
using System.Collections.Generic;
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
    /// <summary>
    ///     Tests for <see cref="MockQueryProvider"/> descendants.
    /// </summary>
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
    public sealed class MockQueryProviderTests
    {
        ITestOutputHelper Output { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="MockQueryProviderTests"/>.
        /// </summary>
        /// <param name="output">
        ///     Output.
        /// </param>
        public MockQueryProviderTests(ITestOutputHelper output) => Output = output;

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
        ///     Mocked queries mapped by global <see cref="Guid"/> identifier.
        /// </summary>
        [Fact]
        public void TestFakeWithGuid()
        {
            var query1 = new Guid("74F04AEB-6C26-4967-A046-800DD6F85BC5");
            var query2 = new Guid("D5CCA8F0-CC55-4215-9952-5A5471200C02");
            var provider = new MockByIdQueryProvider
            {
                [$"{query1}"] = Product,
                [$"{query2}"] = Product,
            };
            
            (int id0, string code0) = provider
                .Query<int, string>(query1.IdFor($@"
                    |select productid,
                    |       code
                    |  from product"))
                .First();
            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);
            
            (int id1, string code1) = provider
                .Query<int, string>(query2.IdFor(
                    $"select productid, code from product"))
                .Last();
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);
        }

        /// <summary>
        ///     Mocked queries mapped by local <see cref="int"/> identifier.
        /// </summary>
        [Fact]
        public void TestFakeWithInt()
        {
            const int query1 = 42;
            const int query2 = 1492;
            var provider = new MockByIdQueryProvider
            {
                [nameof(TestFakeWithInt), $"{query1}"] = Product,
                [nameof(TestFakeWithInt), $"{query2}"] = Product,
            };
            
            (int id0, string code0) = provider
                .Query<int, string>(query1.LocalIdFor($@"
                    |select productid,
                    |       code
                    |  from product"))
                .First();
            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);

            (int id1, string code1) = provider
                .Query<int, string>(query2.LocalIdFor(
                    $"select productid, code from product"))
                .Last();
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);
        }

        /// <summary>
        ///     Mocked queries mapped by regular expressions.
        /// </summary>
        [Fact]
        public void TestFakeWithRegex()
        {
            var provider = new MockByPatternQueryProvider
            {
                ["productid.+code.+product"] = Product
            };

            (int id0, string code0) = provider
                .Query<int, string>($@"
                    |select productid,
                    |       code
                    |  from product")
                .First();
            Assert.Equal(Product[0][0], id0);
            Assert.Equal(Product[0][1], code0);

            (int id1, string code1) = provider
                .Query<int, string>(
                    $"select productid, code from product")
                .Last();
            Assert.Equal(Product[^1][0], id1);
            Assert.Equal(Product[^1][1], code1);
        }

        /// <summary>
        ///     Serializing queries.
        /// </summary>
        [Fact]
        public void TestMockSerialize()
        {
            const int numb = 100000;
            const string str = "x";
            var mockByPatternQueryProvider = new MockByPatternQueryProvider();
            using var writer = new StringWriter();
            mockByPatternQueryProvider.Read<DateTime?>(1.IdFor($@"
                |select productid
                |  from product
                | where 'xxx' {str:=*}
                |   and 42 {null:!=}
                |   and productid > {numb}"));
            mockByPatternQueryProvider.SaveQueries(writer);
            mockByPatternQueryProvider.Clear();
            var xml = writer.ToString();
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
        }
    }
}
