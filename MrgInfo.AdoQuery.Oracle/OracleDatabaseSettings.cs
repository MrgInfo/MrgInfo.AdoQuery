using System;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace Sda.Query
{
    /// <summary>
    ///     Oracle adatbázis-kiszolgálóhoz kapcsolódás.
    /// </summary>
    /// <inheritdoc />
    public sealed class OracleDatabaseSettings: DatabaseSettings
    {
        /// <inheritdoc />
        public OracleDatabaseSettings(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        ///     Konstruktor gyártómetódussal.
        /// </summary>
        /// <param name="builder">
        ///     Kapcsolati leíró összeállításához használt gyártómetódus.
        /// </param>
        /// <inheritdoc />
        public OracleDatabaseSettings(OracleConnectionStringBuilder builder)
            : base((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        public override DbConnection CreateConnection() => new OracleConnection(ConnectionString);

        /// <inheritdoc />
        public override string CreateParameterName(int index) => $":parameter_{GetParameterNumber(index)}";
    }
}
