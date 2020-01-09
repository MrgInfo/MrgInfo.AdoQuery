using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc />
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public abstract class DatabaseSettings: IDatabaseSettings
    {
        /// <summary>
        ///     Paraméterek számozása.
        /// </summary>
        /// <param name="index">
        ///     Paraméter pozíciója.
        /// </param>
        /// <returns>
        ///     A formázott számozás.
        /// </returns>
        ///  <exception cref="ArgumentOutOfRangeException">
        ///     A <paramref name="index" /> értéke érvénytelen.
        /// </exception>
        protected static string GetParameterNumber(int index)
        {
            if (index < 0 || 98 < index) throw new ArgumentOutOfRangeException(nameof(index), index, "0-98");
            return $"{index + 1:00}";
        }

        /// <summary>
        ///     Adatbázis kapcsolati leíró.
        /// </summary>
        protected string ConnectionString { get; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="connectionString">
        ///     Adatbázis kapcsolati leíró.
        /// </param>
        protected DatabaseSettings(string connectionString) =>
            ConnectionString = connectionString;

        /// <inheritdoc />
        public abstract DbConnection CreateConnection();

        /// <inheritdoc />
        public abstract string CreateParameterName(int index);

        /// <inheritdoc />
        public override string ToString() => ConnectionString;
    }
}
