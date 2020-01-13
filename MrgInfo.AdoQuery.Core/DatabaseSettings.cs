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
        ///     Format parameter indexing.
        /// </summary>
        /// <param name="index">
        ///     Index of parameter.
        /// </param>
        /// <returns>
        ///     Formatted parameter index.
        /// </returns>
        ///  <exception cref="ArgumentOutOfRangeException">
        ///     The <paramref name="index" /> is invalid.
        /// </exception>
        protected static string GetParameterNumber(int index)
        {
            if (index < 0 || 98 < index) throw new ArgumentOutOfRangeException(nameof(index), index, "0-98");
            return $"{index + 1:00}";
        }

        /// <summary>
        ///     Connection string.
        /// </summary>
        protected string ConnectionString { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="DatabaseSettings"/>.
        /// </summary>
        /// <param name="connectionString">
        ///     Connection string.
        /// </param>
        protected DatabaseSettings(string connectionString) => ConnectionString = connectionString;

        /// <inheritdoc />
        public abstract DbConnection CreateConnection();

        /// <inheritdoc />
        public abstract string CreateParameterName(int index);

        /// <inheritdoc />
        public override string ToString() => ConnectionString;
    }
}
