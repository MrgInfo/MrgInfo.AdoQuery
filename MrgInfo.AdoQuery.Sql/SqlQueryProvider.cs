using System;
using System.Data.Common;
using System.Data.SqlClient;
using MrgInfo.AdoQuery.Core;

namespace MrgInfo.AdoQuery.Sql
{
    /// <inheritdoc />
    /// <summary>
    ///     Query SQL Server.
    /// </summary>
    public sealed class SqlQueryProvider: DbQueryProvider
    {
        string ConnectionString { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="SqlQueryProvider"/>.
        /// </summary>
        /// <param name="connectionString">
        ///     Connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="connectionString"/> argument has <c>null</c> value.
        /// </exception>
        public SqlQueryProvider(string connectionString) =>
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        /// <summary>
        ///     Initializes a new instance of <see cref="SqlQueryProvider"/>.
        /// </summary>
        /// <param name="builder">
        ///     Builder for connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="builder"/> argument has <c>null</c> value.
        /// </exception>
        public SqlQueryProvider(SqlConnectionStringBuilder builder)
            : this((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        protected override DbConnection CreateConnection() => new SqlConnection(ConnectionString);

        /// <inheritdoc />
        protected  override string CreateParameterName(int index) => $"@Parameter{GetParameterNumber(index)}";

        /// <inheritdoc />
        public override string ToString() => ConnectionString;
    }
}
