using System;
using System.Data.Common;
using System.Data.SqlClient;
using MrgInfo.AdoQuery.Core;

namespace MrgInfo.AdoQuery.Sql
{
    /// <inheritdoc />
    /// <summary>
    ///     MsSQL adatbázis-kiszolgálóhoz kapcsolódás.
    /// </summary>
    public sealed class SqlDatabaseSettings: DatabaseSettings
    {
        /// <inheritdoc />
        public SqlDatabaseSettings(string connectionString)
            : base(connectionString)
        { }

        /// <inheritdoc />
        /// <summary>
        ///     Konstruktor gyártómetódussal.
        /// </summary>
        /// <param name="builder">
        ///     Kapcsolati leíró összeállításához használt gyártómetódus.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="builder"/> értéke null.
        /// </exception>
        public SqlDatabaseSettings(SqlConnectionStringBuilder builder)
            : base((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        public override DbConnection CreateConnection() => new SqlConnection(ConnectionString);

        /// <inheritdoc />
        public override string CreateParameterName(int index) => $"@Parameter{GetParameterNumber(index)}";
    }
}
