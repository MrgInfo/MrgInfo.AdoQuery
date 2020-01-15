using System;
using System.Data.Common;
using MrgInfo.AdoQuery.Core;
using Oracle.ManagedDataAccess.Client;

namespace MrgInfo.AdoQuery.Oracle
{
    /// <inheritdoc />
    /// <summary>
    ///     Query Oracle Database.
    /// </summary>
    public sealed class OracleQueryProvider: DbQueryProvider
    {
        string ConnectionString { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="OracleQueryProvider"/>.
        /// </summary>
        /// <param name="connectionString">
        ///     Connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="connectionString"/> argument has <c>null</c> value.
        /// </exception>
        public OracleQueryProvider(string connectionString) =>
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        /// <summary>
        ///     Initializes a new instance of <see cref="OracleQueryProvider"/>.
        /// </summary>
        /// <param name="builder">
        ///     Builder for connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="builder"/> argument has <c>null</c> value.
        /// </exception>
        public OracleQueryProvider(OracleConnectionStringBuilder builder)
            : this((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        protected override DbConnection CreateConnection() => new OracleConnection(ConnectionString);

        /// <inheritdoc />
        protected override string CreateParameterName(int index) => $":parameter_{GetParameterNumber(index)}";

        /// <inheritdoc />
        public override string ToString() => ConnectionString;
    }
}
