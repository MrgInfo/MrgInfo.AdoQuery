using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MrgInfo.AdoQuery.Core;
using MrgInfo.AdoQuery.Oracle;
using MrgInfo.AdoQuery.Sql;
using Xunit;

namespace MrgInfo.AdoQuery.Test
{
    /// <summary>
    ///     Tests for <see cref="DbQueryProvider"/> descendants.
    /// </summary>
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
    public sealed class DbQueryProviderTests
    {
        /// <summary>
        ///     SQL providers for test.
        /// </summary>
        /// <returns>
        ///     List of providers.
        /// </returns>
        public static IEnumerable<object[]> Vendors
        {
            get
            {
                yield return new object[] { new SqlQueryProvider("Data Source=localhost,1433;User Id=AdoQuery;Password=AdoQuery;") };
                yield return new object[] { new OracleQueryProvider("Data Source=localhost:1521/ORCLCDB.localdomain;User Id=adoquery;Password=adoquery;") };
            }
        }

        /// <summary>
        ///     Run simple query.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestConnection(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var cnt = provider.Read<int>(1.IdFor($@"
                |select count(*)
                |  from product
                | where unitprice > {500}"));
            Assert.Equal(3, cnt);
        }

        /// <summary>
        ///     Test for extra columns (should be filled with default value of the given type).
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestExtraColumns(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var price = 500;
            (int ProductId, string Code, int? Nullable, double Float)[] resultSet =
                provider.Query<int, string, int?, double>($@"
                    |  select productid,
                    |         code
                    |    from product
                    |   where unitprice > {price}
                    |order by productid")
                .ToArray();
            Assert.All(resultSet, row =>
            {
                (_, _, int? nullable, double d) = row;
                Assert.Null(nullable);
                Assert.Equal(default, d);
            });
        }

        /// <summary>
        ///     Test for NULLs.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestNull(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            var code = "PG";
            (int? ProductId, string Name)[] resultSet =
                provider.Query<int?, string>($@"
                    |select productid,
                    |       name
                    |  from product
                    | where name is null
                    |   and code {code:=*}")
                .ToArray();
            Assert.All(resultSet, row =>
            {
                (int? productId, string name) = row;
                Assert.NotNull(productId);
                Assert.Null(name);
            });
        }
    }
}
