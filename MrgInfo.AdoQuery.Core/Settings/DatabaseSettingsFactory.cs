//using System;
//using System.Configuration;

//namespace Sda.Query
//{
//    /// <summary>
//    ///     Az <c>App.config</c> vagy <c>Web.config</c> alapján létrehozza az adatbázis környezetet.
//    /// </summary>
//    public static class DatabaseSettingsFactory
//    {
//        /// <summary>
//        ///     Adatbázis környezet automatikus beállítása.
//        /// </summary>
//        /// <param name="connectionString">
//        ///     Adatbázis kapcsolatot leíró osztály.
//        /// </param>
//        /// <returns>
//        /// </returns>
//        /// <exception cref="ArgumentNullException">
//        ///     A <paramref name="connectionString"/> értéke null.
//        /// </exception>
//        /// <exception cref="ConfigurationErrorsException">
//        ///     Nem ismert adatbázis-kezelő.
//        /// </exception>
//        public static DatabaseSettings CreateSettings(this ConnectionStringSettings connectionString)
//        {
//            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
//            if (connectionString.ConnectionString == null) throw new ArgumentNullException(nameof(connectionString));

//            switch (connectionString.ProviderName)
//            {
//                case "System.Data.SqlClient":
//                    return new SqlDatabaseSettings(connectionString.ConnectionString);
//                case "Oracle.ManagedDataAccess.Client":
//                    return new OracleDatabaseSettings(connectionString.ConnectionString);
//                case "Devart.Data.Oracle":
//                    return new DevartDatabaseSettings(connectionString.ConnectionString);
//                case "Sap.Data.Hana":
//                    return new HanaDatabaseSettings(connectionString.ConnectionString);
//                default:
//                    throw new ConfigurationErrorsException();
//            }
//        }

//        /// <summary>
//        ///     Adatbázis környezet automatikus beállítása.
//        /// </summary>
//        /// <param name="connectionName">
//        ///     Adatbázis kapcsolatot neve az <c>App.config</c>-ban vagy <c>Web.config</c>-ban.
//        /// </param>
//        /// <returns>
//        /// </returns>
//        /// <exception cref="ArgumentNullException">
//        ///     A <paramref name="connectionName"/> értéke null.
//        /// </exception>
//        /// <exception cref="ConfigurationErrorsException">
//        ///     Nem ismert adatbázis-kezelő.
//        /// </exception>
//        public static DatabaseSettings CreateSettings([NotNull] string connectionName) =>
//            ConfigurationManager.ConnectionStrings?[connectionName ?? throw new ArgumentNullException(nameof(connectionName))]?.CreateSettings()
//            ?? throw new ConfigurationErrorsException();
//    }
//}
