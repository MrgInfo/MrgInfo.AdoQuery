using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.StringComparison;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Run SQL queries.
    /// </summary>
    public abstract class QueryProvider
    {
        

        static string? FindId(FormattableString? formattableString) =>
            formattableString is FormattableStringWithId withInt
                ? withInt.Id
                : null;

        /// <summary>
        ///     Removes trailing whitespace before | character.
        /// </summary>
        /// <param name="query">
        ///     Query to format.
        /// </param>
        /// <returns>
        ///     Formatted query.
        /// </returns>
        protected static string RemoveTrailing(string? query)
        {
            if (query == null) return "";
            var result = new StringBuilder();
            foreach (string shortLine in
                from line in query.Split('\n')
                let shortLine = line?.Trim()
                where ! string.IsNullOrEmpty(shortLine)
                select shortLine)
            {
                if (result.Length > 0) result.Append('\n');
                result.Append(shortLine.StartsWith("|", OrdinalIgnoreCase)
                    ? shortLine.Substring(1)
                    : shortLine);
            }
            return result.ToString();
        }

        /// <summary>
        ///     Universal cast / convert function.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The required type.
        /// </typeparam>
        /// <param name="value">
        ///     The value to cast or convert.
        /// </param>
        /// <returns>
        ///     <paramref name="value"/> converted to <typeparamref name="TResult"/> type or default if <c>null</c>.
        /// </returns>
        /// <exception cref="InvalidCastException">
        ///     Converting <paramref name="value"/> to <typeparamref name="TResult"/> is not possible.
        /// </exception>
        [SuppressMessage("ReSharper", "InvertIf")]
        [return: MaybeNull]
        protected static TResult Cast<TResult>(object? value)
        {
            Type type = typeof(TResult);
            if (type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
            }
            if (value is null || value is DBNull)
            {
                return default!;
            }
            if (type == typeof(bool) || type == typeof(bool?))
            {
                if ("T".Equals(value.ToString(), OrdinalIgnoreCase))
                {
                    value = true;
                }
                else if ("F".Equals(value.ToString(), OrdinalIgnoreCase))
                {
                    value = false;
                }
            }
            return (TResult)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Run query synchronously.
        /// </summary>
        /// <param name="id">
        ///     The unique identifier of query.
        /// </param>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="parameters">
        ///     Query parameters.
        /// </param>
        /// <param name="columns">
        ///     Number of column is result set.
        /// </param>
        /// <returns>
        ///     Result set.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid number of columns in <paramref name="columns"/>.
        /// </exception>
        protected internal abstract IEnumerable<object?[]> Query(string? id, string? query, IReadOnlyList<object?>? parameters, int columns);

        IEnumerable<object?[]> SafeQuery(string? id, string? query, IReadOnlyList<object?>? parameters, int columns)
        {
            try
            {
                return Query(id, query, parameters, columns);
            }
            catch (DbException exp)
            {
                throw new QueryDbException(id, RemoveTrailing(query), parameters, exp);
            }
        }

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<T1>
        Query<T1>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 1)
            select Cast<T1>(v[0]);

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2)>
        Query<T1, T2>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 2)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3)>
        Query<T1, T2, T3>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 3)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>
            Query<T1, T2, T3, T4>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format,
                query.GetArguments(), 4)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>
        Query<T1, T2, T3, T4, T5>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 5)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6)>
        Query<T1, T2, T3, T4, T5, T6>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 6)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7)>
        Query<T1, T2, T3, T4, T5, T6, T7>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 7)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 8)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 9)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 olumn8, T9 Column9, T10 Column10)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 10)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 11)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 12)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 13)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 14)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/columns/*[15]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FormattableString? query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 15)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]));

        /// <include file='Documentation.xml' path='docs/query/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/columns/*[15]' />
        /// <include file='Documentation.xml' path='docs/columns/*[16]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15, T16 Column16)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FormattableString query) =>
            from v in SafeQuery(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), 16)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]), Cast<T16>(v[15]));

        /// <summary>
        ///     Run query asynchronously.
        /// </summary>
        /// <param name="id">
        ///     The unique identifier of query.
        /// </param>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="parameters">
        ///     Query parameters.
        /// </param>
        /// <param name="columns">
        ///     Number of column is result set.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <returns>
        ///     Result set.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid number of columns in <paramref name="columns"/>.
        /// </exception>
        protected internal abstract IAsyncEnumerable<object?[]> QueryAsync(string? id, string? query, IReadOnlyList<object?>? parameters, int columns, CancellationToken token = default);

        async IAsyncEnumerable<object?[]> SafeQueryAsync(string? id, string? query, IReadOnlyList<object?>? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            IAsyncEnumerator<object?[]> enumerator;
            try
            {
                enumerator = QueryAsync(id, query, parameters, columns, token).GetAsyncEnumerator(token);
            }
            catch (DbException exp)
            {
                throw new QueryDbException(id, RemoveTrailing(query), parameters, exp);
            }

            try
            {
                while (true)
                {
                    bool hasValue;
                    try
                    {
                        hasValue = await enumerator.MoveNextAsync();
                    }
                    catch (DbException exp)
                    {
                        throw new QueryDbException(id, RemoveTrailing(query), parameters, exp);
                    }

                    if (!hasValue) yield break;
                    yield return enumerator.Current;
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<T1>
        QueryAsync<T1>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 1, token).ConfigureAwait(false))
            {
                yield return Cast<T1>(v[0]);
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2)>
        QueryAsync<T1, T2>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 2, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3)>
        QueryAsync<T1, T2, T3>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 3, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>
        QueryAsync<T1, T2, T3, T4>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 4, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>
        QueryAsync<T1, T2, T3, T4, T5>(FormattableString query,
                [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 5, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 9, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 10, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 11, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 12, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
                /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 13, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 14, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/columns/*[15]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 15, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]));
            }
        }

        /// <include file='Documentation.xml' path='docs/queryasync/*' />
        /// <include file='Documentation.xml' path='docs/columns/*[1]' />
        /// <include file='Documentation.xml' path='docs/columns/*[2]' />
        /// <include file='Documentation.xml' path='docs/columns/*[3]' />
        /// <include file='Documentation.xml' path='docs/columns/*[4]' />
        /// <include file='Documentation.xml' path='docs/columns/*[5]' />
        /// <include file='Documentation.xml' path='docs/columns/*[6]' />
        /// <include file='Documentation.xml' path='docs/columns/*[7]' />
        /// <include file='Documentation.xml' path='docs/columns/*[8]' />
        /// <include file='Documentation.xml' path='docs/columns/*[9]' />
        /// <include file='Documentation.xml' path='docs/columns/*[10]' />
        /// <include file='Documentation.xml' path='docs/columns/*[11]' />
        /// <include file='Documentation.xml' path='docs/columns/*[12]' />
        /// <include file='Documentation.xml' path='docs/columns/*[13]' />
        /// <include file='Documentation.xml' path='docs/columns/*[14]' />
        /// <include file='Documentation.xml' path='docs/columns/*[15]' />
        /// <include file='Documentation.xml' path='docs/columns/*[16]' />
        /// <include file='Documentation.xml' path='docs/format/*' />
        public async IAsyncEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15, T16 Column16)>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FormattableString query, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            await foreach (object?[] v in SafeQueryAsync(FindId(query), query.Format, query.GetArguments(), 16, token).ConfigureAwait(false))
            {
                yield return (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]), Cast<T16>(v[15]));
            }
        }

        /// <summary>
        ///     Run query synchronously with scalar result.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of result.
        /// </typeparam>
        /// <param name="id">
        ///     The unique identifier of query.
        /// </param>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="parameters">
        ///     Query parameters.
        /// </param>
        /// <returns>
        ///     Scalar value.
        /// </returns>
        [return: MaybeNull]
        protected internal abstract TResult Read<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters);

        TResult SafeRead<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters)
        {
            try
            {
                return Read<TResult>(id, query, parameters);
            }
            catch (DbException exp)
            {
                throw new QueryDbException(id, RemoveTrailing(query), parameters, exp);
            }
        }

        /// <summary>
        ///     Run query synchronously with scalar result.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of result.
        /// </typeparam>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <returns>
        ///     Scalar value.
        /// </returns>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        /// <include file='Documentation.xml' path='docs/format/*'/>
        public TResult Read<TResult>(FormattableString query) =>
            SafeRead<TResult>(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments());

        /// <summary>
        ///     Run query asynchronously with scalar result.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of result.
        /// </typeparam>
        /// <param name="id">
        ///     The unique identifier of query.
        /// </param>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="parameters">
        ///     Query parameters.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <returns>
        ///     Scalar value.
        /// </returns>
        protected internal abstract Task<TResult> ReadAsync<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters, CancellationToken token = default);

        async Task<TResult> SafeReadAsync<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters, CancellationToken token = default)
        {
            try
            {
                return await ReadAsync<TResult>(id, query, parameters, token).ConfigureAwait(false);
            }
            catch (DbException exp)
            {
                throw new QueryDbException(id, RemoveTrailing(query), parameters, exp);
            }
        }

        /// <summary>
        ///     Run query asynchronously with scalar result.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of result.
        /// </typeparam>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="token">
        ///    The cancellation token that will be checked for stop reading.
        /// </param>
        /// <returns>
        ///     Scalar value.
        /// </returns>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        /// <include file='Documentation.xml' path='docs/format/*'/>
        public Task<TResult> ReadAsync<TResult>(FormattableString query, CancellationToken token = default) =>
            SafeReadAsync<TResult>(FindId(query ?? throw new ArgumentNullException(nameof(query))), query.Format, query.GetArguments(), token);
    }
}
