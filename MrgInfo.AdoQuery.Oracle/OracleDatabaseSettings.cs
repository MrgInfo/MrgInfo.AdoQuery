using System;
using System.Data.Common;
using MrgInfo.AdoQuery.Core;
using Oracle.ManagedDataAccess.Client;

namespace MrgInfo.AdoQuery.Oracle
{
    /// <inheritdoc />
    /// <summary>
    ///     Oracle adatbázis-kiszolgálóhoz kapcsolódás.
    /// </summary>
    public sealed class OracleDatabaseSettings: DatabaseSettings
    {
        /// <inheritdoc />
        public OracleDatabaseSettings(string connectionString)
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
        public OracleDatabaseSettings(OracleConnectionStringBuilder builder)
            : base((builder ?? throw new ArgumentNullException(nameof(builder))).ToString())
        { }

        /// <inheritdoc />
        public override DbConnection CreateConnection() => new OracleConnection(ConnectionString);

        /// <inheritdoc />
        public override string CreateParameterName(int index) => $":parameter_{GetParameterNumber(index)}";
    }
}
