using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sda.Query
{
    /// <summary>
    ///     MsSQL adatbázis-kiszolgálóhoz kapcsolódás.
    /// </summary>
    /// <inheritdoc />
    public sealed class SqlDatabaseSettings: DatabaseSettings
    {
        /// <inheritdoc />
        public SqlDatabaseSettings(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        ///     Konstruktor gyártómetódussal.
        /// </summary>
        /// <param name="builder">
        ///     Kapcsolati leíró összeállításához használt gyártómetódus.
        /// </param>
        /// <inheritdoc />
        public SqlDatabaseSettings(SqlConnectionStringBuilder builder)
            : base((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        public override DbConnection CreateConnection() => new SqlConnection(ConnectionString);

        /// <inheritdoc />
        public override string CreateParameterName(int index) => $"@Parameter{GetParameterNumber(index)}";
    }
}
