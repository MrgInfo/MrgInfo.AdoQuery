﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
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
        public static IEnumerable<object[]> Vendors()
        {
            yield return new object[]
            {
                new SqlQueryProvider(new SqlConnectionStringBuilder
                {
                    DataSource = "localhost,1433",
                    UserID = "AdoQuery",
                    Password = "AdoQuery",
                    ConnectRetryCount = 3,
                    ConnectRetryInterval = 30,
                    ConnectTimeout = 120,
                    MultipleActiveResultSets = true,
                })
            };
            yield return new object[] 
            { 
                new OracleQueryProvider(string.Join(';',
                    "Data Source=localhost:1521/ORCLCDB.localdomain",
                    "User Id=adoquery",
                    "Password=adoquery"))
            };
        }

        /// <summary>
        ///     Run simple query.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public void TestRead(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            int cnt = provider.Read<int>(1.IdFor($@"
                |select count(*)
                |  from product
                | where unitprice > {500}"));
            Assert.Equal(3, cnt);
        }

        /// <summary>
        ///     Run simple query.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public async Task TestReadAsync(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            int cnt = await provider.ReadAsync<int>(1.IdFor($@"
                |select count(*)
                |  from product
                | where unitprice > {500}"))
                .ConfigureAwait(false);
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

            const int price = 500;
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

            const string code = "PG";
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

        /// <summary>
        ///     Test for NULLs.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        [Theory, MemberData(nameof(Vendors))]
        public async Task TestNullAsync(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            const string code = "PG";
            (int? ProductId, string Name)[] resultSet =
                await provider.QueryAsync<int?, string>($@"
                    |select productid,
                    |       name
                    |  from product
                    | where name is null
                    |   and code {code:=*}")
                    .ToArrayAsync()
                    .ConfigureAwait(false);
            Assert.All(resultSet, row =>
            {
                (int? productId, string name) = row;
                Assert.NotNull(productId);
                Assert.Null(name);
            });
        }
    }
}
