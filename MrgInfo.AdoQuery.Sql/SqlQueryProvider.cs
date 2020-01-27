using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using MrgInfo.AdoQuery.Core;

namespace MrgInfo.AdoQuery.Sql
{
    /// <inheritdoc />
    /// <summary>
    ///     Query SQL Server.
    /// </summary>
    public sealed class SqlQueryProvider: DbQueryProvider
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="SqlQueryProvider"/>.
        /// </summary>
        public SqlQueryProvider()
            : base("")
        { }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of <see cref="SqlQueryProvider"/>.
        /// </summary>
        public SqlQueryProvider(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        ///     Initializes a new instance of <see cref="SqlQueryProvider"/>.
        /// </summary>
        /// <param name="builder">
        ///     Builder for connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="builder"/> argument has <c>null</c> value.
        /// </exception>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public SqlQueryProvider(SqlConnectionStringBuilder builder)
            : base(builder)
        { }

        /// <inheritdoc />
        protected override DbConnection CreateConnection() => new SqlConnection(ConnectionString);

        /// <inheritdoc />
        protected  override string CreateParameterName(int index) => $"@Parameter{GetParameterNumber(index)}";
    }
}
