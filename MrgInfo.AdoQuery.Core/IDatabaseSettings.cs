using System;
using System.Data.Common;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Adatbázis lekérdezéshez szükséges beállítások.
    /// </summary>
    public interface IDatabaseSettings
    {
        /// <summary>
        ///     Új adatbázis kapcsolat létrehozása.
        /// </summary>
        /// <returns>
        ///     Elérhető adatbázis kapcsolat.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis kapcsolat nem hozható létre.
        /// </exception>
        DbConnection CreateConnection();

        /// <summary>
        ///     Egy nevet generál az adott sorszámú <c>SQL</c> paraméternek.
        /// </summary>
        /// <param name="index">
        ///     Az <c>SQL</c> paraméter sorszám.
        /// </param>
        /// <returns>
        ///     Az <c>SQL</c> paraméter neve.
        /// </returns>
        ///  <exception cref="ArgumentOutOfRangeException">
        ///     A <paramref name="index" /> értéke érvénytelen.
        /// </exception>
        string CreateParameterName(int index);
    }
}
