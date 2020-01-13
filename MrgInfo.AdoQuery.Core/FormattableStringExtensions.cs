using System;
using System.Runtime.CompilerServices;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Assigning unique identifier for a query.
    /// </summary>
    public static class FormattableStringExtensions
    {
        /// <summary>
        ///     Adding universal unique identifier for query.
        /// </summary>
        /// <param name="id">
        ///     Unique identifier for query.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The query with identifier.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="query"/> argument has <c>null</c> value.
        /// </exception>
        public static FormattableString IdFor<TId>(this TId id, FormattableString query) where TId: struct =>
            new FormattableStringWithId($"{id}", query ?? throw new ArgumentNullException(nameof(query)));

        /// <summary>
        ///     Adding unique identifier for query within caller context.
        /// </summary>
        /// <param name="id">
        ///     Unique identifier for query.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="memberName">
        ///     The caller member.
        /// </param>
        /// <returns>
        ///     The query with identifier.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="query" /> or <paramref name="memberName" /> argument has <c>null</c> value.
        /// </exception>
        public static FormattableString LocalIdFor<TId>(this TId id, FormattableString query, [CallerMemberName] string memberName = "") where TId: struct =>
            new FormattableStringWithId(string.IsNullOrEmpty(memberName) ? $"$/{id}" : $"{memberName}/{id}", query ?? throw new ArgumentNullException(nameof(query)));
    }
}
