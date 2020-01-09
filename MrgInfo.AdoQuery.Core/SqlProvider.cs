using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core.FormattableStrings;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Lekérdezések futtatása.
    /// </summary>
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class SqlProvider
    {
        /// <summary>
        ///     Lekérdezés 1 oszloppal.
        /// </summary>
        /// <typeparam name="T">
        ///     Az oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<T> Query<T>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 1)
            select Cast<T>(v[0]);

        /// <summary>
        ///     Lekérdezés 1 oszloppal.
        /// </summary>
        /// <typeparam name="T">
        ///     Az oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <include file='Documentation.xml' path='docs/sqlformat/*' />
        public async Task<IEnumerable<T>> QueryAsync<T>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 1, token).ConfigureAwait(false)
            select Cast<T>(v[0]);

        /// <summary>
        ///     Lekérdezés 2 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(T1 Column1, T2 Column2)>
        Query<T1, T2>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 2)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]));

        /// <summary>
        ///     Lekérdezés 2 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(T1 Column1, T2 Column2)>>
        QueryAsync<T1, T2>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 2, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]));

        /// <summary>
        ///     Lekérdezés 3 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3)> 
        Query<T1, T2, T3>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 3)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));

        /// <summary>
        ///     Lekérdezés 3 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3)>>
        QueryAsync<T1, T2, T3>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 3, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));

        /// <summary>
        ///     Lekérdezés 4 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>
        Query<T1, T2, T3, T4>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 4)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));

        /// <summary>
        ///     Lekérdezés 4 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>>
        QueryAsync<T1, T2, T3, T4>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 4, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));

        /// <summary>
        ///     Lekérdezés 5 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>
        Query<T1, T2, T3, T4, T5>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 5)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));

        /// <summary>
        ///     Lekérdezés 5 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>>
        QueryAsync<T1, T2, T3, T4, T5>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 5, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));

        /// <summary>
        ///     Lekérdezés 6 oszloppal.
        /// </summary>
        /// <typeparam name="T1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6)>
        Query<T1, T2, T3, T4, T5, T6>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 6)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]));

        /// <summary>
        ///     Lekérdezés 6 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(TColumn1 Column1, TColumn2 Column2, TColumn3 Column3, TColumn4 Column4, TColumn5 Column5, TColumn6 Column6)>>
        QueryAsync<TColumn1, TColumn2, TColumn3, TColumn4, TColumn5, TColumn6>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 6, token).ConfigureAwait(false)
            select (Cast<TColumn1>(v[0]), Cast<TColumn2>(v[1]), Cast<TColumn3>(v[2]), Cast<TColumn4>(v[3]), Cast<TColumn5>(v[4]), Cast<TColumn6>(v[5]));

        /// <summary>
        ///     Lekérdezés 7 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(TColumn1 Column1, TColumn2 Column2, TColumn3 Column3, TColumn4 Column4, TColumn5 Column5, TColumn6 Column6, TColumn7 Column7)>
        Query<TColumn1, TColumn2, TColumn3, TColumn4, TColumn5, TColumn6, TColumn7>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 7)
            select (Cast<TColumn1>(v[0]), Cast<TColumn2>(v[1]), Cast<TColumn3>(v[2]), Cast<TColumn4>(v[3]), Cast<TColumn5>(v[4]), Cast<TColumn6>(v[5]), Cast<TColumn7>(v[6]));

        /// <summary>
        ///     Lekérdezés 7 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(TColumn1 Column1, TColumn2 Column2, TColumn3 Column3, TColumn4 Column4, TColumn5 Column5, TColumn6 Column6, TColumn7 Column7)>>
        QueryAsync<TColumn1, TColumn2, TColumn3, TColumn4, TColumn5, TColumn6, TColumn7>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 7, token).ConfigureAwait(false)
            select (Cast<TColumn1>(v[0]), Cast<TColumn2>(v[1]), Cast<TColumn3>(v[2]), Cast<TColumn4>(v[3]), Cast<TColumn5>(v[4]), Cast<TColumn6>(v[5]), Cast<TColumn7>(v[6]));

        /// <summary>
        ///     Lekérdezés 8 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(TColumn1 Column1, TColumn2 Column2, TColumn3 Column3, TColumn4 Column4, TColumn5 Column5, TColumn6 Column6, TColumn7 Column7, TColumn8 Column8)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 8)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]));

        /// <summary>
        ///     Lekérdezés 8 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 8, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]));

        /// <summary>
        ///     Lekérdezés 9 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 9)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]));

        /// <summary>
        ///     Lekérdezés 9 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 9, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]));

        /// <summary>
        ///     Lekérdezés 10 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 10)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]));

        /// <summary>
        ///     Lekérdezés 10 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 10, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]));

        /// <summary>
        ///     Lekérdezés 11 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 11)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]));

        /// <summary>
        ///     Lekérdezés 11 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 11, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]));

        /// <summary>
        ///     Lekérdezés 12 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 12)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]));

        /// <summary>
        ///     Lekérdezés 12 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 12, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]));

        /// <summary>
        ///     Lekérdezés 13 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 13)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]));

        /// <summary>
        ///     Lekérdezés 13 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 13, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]));

        /// <summary>
        ///     Lekérdezés 14 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 14)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]));

        /// <summary>
        ///     Lekérdezés 14 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 14, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]));

        /// <summary>
        ///     Lekérdezés 15 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn15">
        ///     Az 15. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14,
                TColumn15 Column15)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14,
                TColumn15>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 15)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]),
                Cast<TColumn15>(v[14]));

        /// <summary>
        ///     Lekérdezés 15 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn15">
        ///     Az 15. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14,
                TColumn15 Column15)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14,
                TColumn15>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 15, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]),
                Cast<TColumn15>(v[14]));

        /// <summary>
        ///     Lekérdezés 16 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn15">
        ///     Az 15. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn16">
        ///     Az 16. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14,
                TColumn15 Column15,
                TColumn16 Column16)>
            Query<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14,
                TColumn15,
                TColumn16>
            (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 16)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]),
                Cast<TColumn15>(v[14]),
                Cast<TColumn16>(v[15]));

        /// <summary>
        ///     Lekérdezés 16 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn3">
        ///     Az 3. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn4">
        ///     Az 4. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn5">
        ///     Az 5. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn6">
        ///     Az 6. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn7">
        ///     Az 7. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn8">
        ///     Az 8. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn9">
        ///     Az 9. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn10">
        ///     Az 10. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn11">
        ///     Az 11. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn12">
        ///     Az 12. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn13">
        ///     Az 13. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn14">
        ///     Az 14. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn15">
        ///     Az 15. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn16">
        ///     Az 16. oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
                TColumn1 Column1,
                TColumn2 Column2,
                TColumn3 Column3,
                TColumn4 Column4,
                TColumn5 Column5,
                TColumn6 Column6,
                TColumn7 Column7,
                TColumn8 Column8,
                TColumn9 Column9,
                TColumn10 Column10,
                TColumn11 Column11,
                TColumn12 Column12,
                TColumn13 Column13,
                TColumn14 Column14,
                TColumn15 Column15,
                TColumn16 Column16)>>
            QueryAsync<
                TColumn1,
                TColumn2,
                TColumn3,
                TColumn4,
                TColumn5,
                TColumn6,
                TColumn7,
                TColumn8,
                TColumn9,
                TColumn10,
                TColumn11,
                TColumn12,
                TColumn13,
                TColumn14,
                TColumn15,
                TColumn16>
            (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 16, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]),
                Cast<TColumn8>(v[7]),
                Cast<TColumn9>(v[8]),
                Cast<TColumn10>(v[9]),
                Cast<TColumn11>(v[10]),
                Cast<TColumn12>(v[11]),
                Cast<TColumn13>(v[12]),
                Cast<TColumn14>(v[13]),
                Cast<TColumn15>(v[14]),
                Cast<TColumn16>(v[15]));

        /// <summary>
        ///     Lekérdezés paraméter.
        /// </summary>
        /// <inheritdoc />
        protected sealed class Parameter: IFormattable
        {
            /// <summary>
            ///     Név.
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            ///     Érték.
            /// </summary>
            public object? Value { get; set; }

            /// <summary>
            ///     Megtalálható a lekérdezésben.
            /// </summary>
            public bool Present { get; set; }

            /// <inheritdoc />
            public override string ToString() => $"{Name ?? "N/A"} = {Value ?? "N/A"}";

            /// <inheritdoc />
            /// <exception cref="FormatException">
            ///     Hibás formázás!
            /// </exception>
            [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider")]
            [SuppressMessage("ReSharper", "InvertIf")]
            public string ToString(string format, IFormatProvider formatProvider)
            {
                switch (format)
                {
                    case "==":
                        if (string.IsNullOrEmpty(Name)) goto case default;
                        if (Value is null || Value is DBNull)
                        {
                            Present = false;
                            return "is null";
                        }
                        Present = true;
                        return $"= {Name}";
                    case "!=":
                        if (string.IsNullOrEmpty(Name)) goto case default;
                        if (Value is null || Value is DBNull)
                        {
                            Present = false;
                            return "is not null";
                        }
                        Present = true;
                        return $"<> {Name}";
                    case "=*":
                        if (string.IsNullOrEmpty(Name)) goto case default;
                        Value = $"{Value}%";
                        Present = true;
                        return $"like {Name}";
                    case "*=":
                        if (string.IsNullOrEmpty(Name)) goto case default;
                        Value = $"%{Value}";
                        Present = true;
                        return $"like {Name}";
                    case null:
                    case "":
                        if (string.IsNullOrEmpty(Name)) goto case default;
                        Present = true;
                        return Name;
                    default:
                        throw new FormatException();
                }
            }
        }

        static TraceSource TraceSource { get; } = new TraceSource(nameof(SqlProvider), SourceLevels.Information);

        /// <summary>
        ///     Garantálja, hogy a paraméterben kapott <paramref name="function"/> függvény vagy sikerese végrehajtódik,
        ///     vagy <see cref="QueryDbException"/> kivételt kapunk.
        /// </summary>
        /// <typeparam name="TResult">
        ///     A végrehajtott függvény visszatérési értékének típusa.
        /// </typeparam>
        /// <param name="id">
        ///     A <see cref="QueryDbException"/> által regisztrált lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="query">
        ///     A <see cref="QueryDbException"/> által regisztrált lekérdezés.
        /// </param>
        /// <param name="parameters">
        ///     A <see cref="QueryDbException"/> által regisztrált paraméterek.
        /// </param>
        /// <param name="function">
        ///     A végrehajtott függvény.
        /// </param>
        /// <returns>
        ///     A <paramref name="function"/> által visszaadott érték.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="function"/> értéke null.
        /// </exception>
        protected static TResult WrapException<TResult>(string? id, string? query, IEnumerable<object>? parameters, Func<TResult> function)
        {
            if (function is null) throw new ArgumentNullException(nameof(function));

            try
            {
                return function.Invoke();
            }
            catch (DbException exp)
            {
                throw QueryDbException.Create(id, RemoveTrailing(query), parameters, exp);
            }
        }

        /// <summary>
        ///     Garantálja, hogy a paraméterben kapott asszinkron <paramref name="function"/> függvény vagy sikerese végrehajtódik,
        ///     vagy <see cref="QueryDbException"/> kivételt kapunk.
        /// </summary>
        /// <typeparam name="TResult">
        ///     A végrehajtott függvény visszatérési értékének típusa.
        /// </typeparam>
        /// <param name="id">
        ///     A <see cref="QueryDbException"/> által regisztrált lekérdezés azonosítója.
        /// </param>
        /// <param name="query">
        ///     A <see cref="QueryDbException"/> által regisztrált lekérdezés.
        /// </param>
        /// <param name="parameters">
        ///     A <see cref="QueryDbException"/> által regisztrált paraméterek.
        /// </param>
        /// <param name="function">
        ///     A végrehajtott függvény.
        /// </param>
        /// <returns>
        ///     A <paramref name="function"/> által visszaadott <see cref="Task"/> feladat.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="function"/> értéke null.
        /// </exception>
        protected static async Task<TResult> WrapExceptionAsync<TResult>(string? query, string? id, IEnumerable<object>? parameters, Func<Task<TResult>> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            try
            {
                return await function.Invoke().ConfigureAwait(false);
            }
            catch (DbException exp)
            {
                throw QueryDbException.Create(id, RemoveTrailing(query), parameters, exp);
            }
        }

        /// <summary>
        ///     Törli a behúzást a | karakterig minden sorban.
        /// </summary>
        /// <param name="query">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Az átformázott lekérdezés.
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
                result.Append(shortLine.StartsWith("|", StringComparison.OrdinalIgnoreCase)
                    ? shortLine.Substring(1)
                    : shortLine);
            }
            return result.ToString();
        }

        static void Print(IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Parameters == null) throw new ArgumentNullException(nameof(command));

            var text = new StringBuilder();
            text.AppendLine()
                .AppendLine(command.CommandText)
                .AppendLine();
            foreach (var parameter in command.Parameters.OfType<IDbDataParameter>())
            {
                text.Append(parameter.ParameterName)
                    .Append(" = ")
                    .Append(parameter.Value)
                    .AppendLine();
            }
            TraceSource.TraceInformation(text.ToString());
        }

        /// <summary>
        ///     Objektum típuskényszerítése, amennyiben lehetséges.
        /// </summary>
        /// <typeparam name="TResult">
        ///     A kényszerített típus.
        /// </typeparam>
        /// <param name="value">
        ///     Az objektum.
        /// </param>
        /// <returns>
        ///     Az új típusú objektum, ha <paramref name="value"/> null referenciák, akkor a
        ///     típus alapértelmezett értéke.
        /// </returns>
        /// <exception cref="InvalidCastException">
        ///     Ha nem lehetséges az adott erős típuskényszerítés.
        /// </exception>
        [SuppressMessage("ReSharper", "InvertIf")]
        [return: MaybeNull]
        protected static TResult Cast<TResult>(object? value)
        {
            var type = typeof(TResult);
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
                if ("T".Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    value = true;
                }
                else if ("F".Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    value = false;
                }
            }
            return (TResult)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        static string? FindId(FormattableString? formattableString) =>
            formattableString is FormattableStringWithId withInt
                ? withInt.Id
                : null;

        readonly IDatabaseSettings _settings;

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="settings">
        ///     Adatbázis beállítások.
        /// </param>
        public SqlProvider(IDatabaseSettings settings) =>
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        DbCommand CreateCommand(DbConnection conncetion, string format, object[] args)
        {
            if (conncetion == null) throw new ArgumentNullException(nameof(conncetion));
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (args == null) throw new ArgumentNullException(nameof(args));

            var command = conncetion.CreateCommand();
            try
            {
                var placeholders = new object[args.Length];
                for (var i = 0; i < args.Length; ++i)
                {
                    var item = new Parameter
                    {
                        Name= _settings.CreateParameterName(i),
                        Value = args[i]
                    };
                    IDbDataParameter parameter = command.CreateParameter();
                    parameter.ParameterName = item.Name;
                    parameter.Direction = ParameterDirection.Input;
                    switch (item.Value)
                    {
                        case bool _:
                            parameter.DbType = DbType.Boolean;
                            break;
                        case char _:
                            parameter.DbType = DbType.StringFixedLength;
                            parameter.Size = 1;
                            break;
                        case int _:
                            parameter.DbType = DbType.Int32;
                            break;
                        case long _:
                            parameter.DbType = DbType.Int64;
                            break;
                        case float _:
                            parameter.DbType = DbType.Single;
                            break;
                        case double _:
                            parameter.DbType = DbType.Double;
                            break;
                        case DateTime _:
                            parameter.DbType = DbType.DateTime;
                            break;
                        case Guid _:
                            parameter.DbType = DbType.Guid;
                            break;
                        case string _:
                            parameter.DbType = DbType.String;
                            break;
                    }
                    command.Parameters.Add(parameter);
                    placeholders[i] = item;
                }
                command.CommandText = string.Format(null, format, placeholders);
                for (int i = placeholders.Length - 1; i >= 0 ; --i)
                {
                    var placeholder = (Parameter)placeholders[i];
                    var parameter = command.Parameters[i];
                    parameter.Value = placeholder.Value ?? DBNull.Value;
                    if (! placeholder.Present) command.Parameters.Remove(parameter);
                }
                Print(command);
                return command;
            }
            catch
            {
                command.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Lekérdezés.
        /// </summary>
        /// <param name="id">
        ///      Az <c>SQL</c> lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az <c>SQL</c> lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="args">
        ///     Paraméterek.
        /// </param>
        /// <param name="columns">
        ///     Oszlopok száma.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     A <paramref name="columns"/> értéke nem pozitív.
        /// </exception>
        protected internal virtual IEnumerable<object[]> Query(string? id, string? format, object[]? args, int columns)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            if (string.IsNullOrEmpty(format)) yield break;
            using var connection = _settings.CreateConnection();
            connection.Open();
            using var command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                yield return values;
            }
        }

        IEnumerable<object[]> SafeQuery(string? id, string? format, object[]? args, int columns) =>
            WrapException(id, format, args, () => Query(id, format, args, columns))
            ?? Enumerable.Empty<object[]>();

        /// <summary>
        ///     Lekérdezés.
        /// </summary>
        /// <param name="id">
        ///      Az <c>SQL</c> lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az <c>SQL</c> lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="args">
        ///     Paraméterek.
        /// </param>
        /// <param name="columns">
        ///     Oszlopok száma.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     A <paramref name="columns"/> értéke nem pozitív.
        /// </exception>
        protected virtual async Task<IEnumerable<object[]>> QueryAsync(string? id, string? format, object[]? args, int columns, CancellationToken token)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            var results = new LinkedList<object[]>();
            if (string.IsNullOrEmpty(format)) return results;
            await using var connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using var command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            await using var reader = await (command.ExecuteReaderAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            while (await (reader?.ReadAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false))
            {
                if (token.IsCancellationRequested) break;
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                results.AddLast(values);
            }
            return results;
        }

        Task<IEnumerable<object[]>> SafeQueryAsync(string? id, string? format, object[]? args, int columns, CancellationToken token) =>
            WrapExceptionAsync(id, format, args, async () => await QueryAsync(id, format, args, columns, token).ConfigureAwait(false));

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="id">
        ///      Az <c>SQL</c> lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az <c>SQL</c> lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="args">
        ///     Paraméterek.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        [return: MaybeNull]
        protected internal virtual TResult Read<TResult>(string? id, string? format, object[]? args)
        {
            if (string.IsNullOrEmpty(format)) return default!;
            using var connection = _settings.CreateConnection();
            connection.Open();
            using IDbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            var result = command.ExecuteScalar();
            if (result is null || result is DBNull) return default!;
            return (TResult)Convert.ChangeType(result, typeof(TResult), CultureInfo.InvariantCulture);
        }

        TResult SafeRead<TResult>(string? id, string? format, object[]? args) =>
            WrapException(id, format, args, () => Read<TResult>(id, format, args));

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public TResult Read<TResult>(FormattableString? sql) =>
            SafeRead<TResult>(FindId(sql), sql?.Format, sql?.GetArguments());

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="result">
        ///     Az adat értéke.
        /// </param>
        /// <returns>
        ///     Sikeres volt-e a lekérdezés?
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public bool TryRead<TResult>(FormattableString sql, [MaybeNull] out TResult result)
        {
            try
            {
                result = Read<TResult>(FindId(sql), sql?.Format, sql?.GetArguments());
                return true;
            }
            catch(DbException)
            {
                result = default!;
                return false;
            }
        }

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="id">
        ///      Az <c>SQL</c> lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az <c>SQL</c> lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="args">
        ///     Paraméterek.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        protected virtual async Task<TResult> ReadAsync<TResult>(string? id, string? format, object[]? args, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(format)) return default!;
            await using var connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using var command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            var result = await (command.ExecuteScalarAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            if (result is null || result is DBNull) return default!;
            return (TResult)Convert.ChangeType(result, typeof(TResult), CultureInfo.InvariantCulture);
        }

        Task<TResult> SafeReadAsync<TResult>(string? id, string? format, object[]? args, CancellationToken token) =>
            WrapExceptionAsync(id, format, args, async () => await ReadAsync<TResult>(id, format, args, token).ConfigureAwait(false));

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="token">
        ///     Megszakítás kezdeményezése.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public Task<TResult> ReadAsync<TResult>(FormattableString? sql, CancellationToken token = default) =>
            SafeReadAsync<TResult>(FindId(sql), sql?.Format, sql?.GetArguments(), token);

        /// <inheritdoc />
        public override string ToString() => _settings.ToString();
    }
}
