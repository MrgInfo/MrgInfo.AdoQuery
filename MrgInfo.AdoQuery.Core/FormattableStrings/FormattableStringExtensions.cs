using System;
using System.Runtime.CompilerServices;

namespace MrgInfo.AdoQuery.Core.FormattableStrings
{
    /// <summary>
    ///     A lekérdezések egyedi beazonosítása.
    /// </summary>
    public static class FormattableStringExtensions
    {
        /// <summary>
        ///     Egyedi azonosító összerendelése egy lekérdezéssel.
        /// </summary>
        /// <param name="id">
        ///     Az egyedi azonosító.
        /// </param>
        /// <param name="sql">
        ///     A lekérdezése.
        /// </param>
        /// <returns>
        ///     A lekérdezés.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Az <paramref name="sql"/> vagy <paramref name="id"/> értéke <c>null</c>.
        /// </exception>
        public static FormattableString Is(this string id, FormattableString sql) =>
            new FormattableStringWithId(
                id ?? throw new ArgumentNullException(nameof(id)),
                sql ?? throw new ArgumentNullException(nameof(sql)));

        /// <summary>
        ///     Egyedi azonosító összerendelése egy lekérdezéssel.
        /// </summary>
        /// <param name="id">
        ///     Az egyedi azonosító.
        /// </param>
        /// <param name="sql">
        ///     A lekérdezése.
        /// </param>
        /// <returns>
        ///     A lekérdezés.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Az <paramref name="sql"/> értéke <c>null</c>.
        /// </exception>
        public static FormattableString Is(this Guid id, FormattableString sql) =>
            new FormattableStringWithId(
                $"{id:N}",
                sql ?? throw new ArgumentNullException(nameof(sql)));

        /// <summary>
        ///     Egyedi azonosító összerendelése egy lekérdezéssel.
        /// </summary>
        /// <param name="id">
        ///     Az egyedi azonosító.
        /// </param>
        /// <param name="sql">
        ///     A lekérdezése.
        /// </param>
        /// <param name="memberName">
        ///     A hívó metódus.
        /// </param>
        /// <returns>
        ///     A lekérdezés.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Az <paramref name="sql"/> értéke <c>null</c>.
        /// </exception>
        public static FormattableString Is(this int id, FormattableString sql, [CallerMemberName] string memberName = "") =>
            new FormattableStringWithId(
                $"{memberName ?? throw new ArgumentNullException(nameof(memberName))}/{id}",
                sql ?? throw new ArgumentNullException(nameof(sql)));
    }
}
