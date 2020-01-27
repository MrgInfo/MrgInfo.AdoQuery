using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
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
        /// <summary>
        ///     Initializes a new instance of <see cref="OracleQueryProvider"/>.
        /// </summary>
        public OracleQueryProvider()
            : base("")
        { }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of <see cref="OracleQueryProvider"/>.
        /// </summary>
        public OracleQueryProvider(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        ///     Initializes a new instance of <see cref="OracleQueryProvider"/>.
        /// </summary>
        /// <param name="builder">
        ///     Builder for connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="builder"/> argument has <c>null</c> value.
        /// </exception>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public OracleQueryProvider(OracleConnectionStringBuilder builder)
            : base(builder)
        { }

        /// <inheritdoc />
        protected override DbConnection CreateConnection() => new OracleConnection(ConnectionString);

        /// <inheritdoc />
        protected override string CreateParameterName(int index) => $":parameter_{GetParameterNumber(index)}";
    }
}
