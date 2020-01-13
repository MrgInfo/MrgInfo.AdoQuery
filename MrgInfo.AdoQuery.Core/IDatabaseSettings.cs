using System;
using System.Data.Common;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Database vendor specific logic.
    /// </summary>
    public interface IDatabaseSettings
    {
        /// <summary>
        ///     Create nem database connection.
        /// </summary>
        /// <returns>
        ///     New connection.
        /// </returns>
        /// <exception cref="DbException">
        ///     Cannot connect to database.
        /// </exception>
        DbConnection CreateConnection();

        /// <summary>
        ///     Unique name for query parameter.
        /// </summary>
        /// <param name="index">
        ///     Index of parameter.
        /// </param>
        /// <returns>
        ///     Unique name of parameter.
        /// </returns>
        ///  <exception cref="ArgumentOutOfRangeException">
        ///     The <paramref name="index" /> is invalid.
        /// </exception>
        string CreateParameterName(int index);
    }
}
