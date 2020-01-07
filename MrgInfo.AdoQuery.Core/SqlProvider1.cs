using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sda.Query
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    partial class SqlProvider
    {
        /// <summary>
        ///     Lekérdezés 2 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
        ///     Az 2. oszlop típusa.
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
            TColumn2 Column2)>
        Query<
            TColumn1,
            TColumn2>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 2)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]));

        /// <summary>
        ///     Lekérdezés 2 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn1">
        ///     Az 1. oszlop típusa.
        /// </typeparam>
        /// <typeparam name="TColumn2">
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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
            TColumn1 Column1,
            TColumn2 Column2)>>
        QueryAsync<
            TColumn1,
            TColumn2>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 2, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]));

        /// <summary>
        ///     Lekérdezés 3 oszloppal.
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
            TColumn3 Column3)>
        Query<
            TColumn1,
            TColumn2,
            TColumn3>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 3)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]));

        /// <summary>
        ///     Lekérdezés 3 oszloppal.
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
            TColumn3 Column3)>>
        QueryAsync<
            TColumn1,
            TColumn2,
            TColumn3>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 3, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]));

        /// <summary>
        ///     Lekérdezés 4 oszloppal.
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
            TColumn4 Column4)>
        Query<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 4)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]));

        /// <summary>
        ///     Lekérdezés 4 oszloppal.
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
            TColumn4 Column4)>>
        QueryAsync<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 4, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]));

        /// <summary>
        ///     Lekérdezés 5 oszloppal.
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
            TColumn5 Column5)>
        Query<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 5)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]));

        /// <summary>
        ///     Lekérdezés 5 oszloppal.
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
            TColumn5 Column5)>>
        QueryAsync<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 5, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]));

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
            TColumn6 Column6)>
        Query<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5,
            TColumn6>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 6)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]));

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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
            TColumn1 Column1,
            TColumn2 Column2,
            TColumn3 Column3,
            TColumn4 Column4,
            TColumn5 Column5,
            TColumn6 Column6)>>
        QueryAsync<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5,
            TColumn6>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 6, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]));

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
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
            TColumn1 Column1,
            TColumn2 Column2,
            TColumn3 Column3,
            TColumn4 Column4,
            TColumn5 Column5,
            TColumn6 Column6,
            TColumn7 Column7)>
        Query<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5,
            TColumn6,
            TColumn7>
        (FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 7)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]));

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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<(
            TColumn1 Column1,
            TColumn2 Column2,
            TColumn3 Column3,
            TColumn4 Column4,
            TColumn5 Column5,
            TColumn6 Column6,
            TColumn7 Column7)>>
        QueryAsync<
            TColumn1,
            TColumn2,
            TColumn3,
            TColumn4,
            TColumn5,
            TColumn6,
            TColumn7>
        (FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 7, token).ConfigureAwait(false)
            select (
                Cast<TColumn1>(v[0]),
                Cast<TColumn2>(v[1]),
                Cast<TColumn3>(v[2]),
                Cast<TColumn4>(v[3]),
                Cast<TColumn5>(v[4]),
                Cast<TColumn6>(v[5]),
                Cast<TColumn7>(v[6]));

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
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<(
            TColumn1 Column1,
            TColumn2 Column2,
            TColumn3 Column3,
            TColumn4 Column4,
            TColumn5 Column5,
            TColumn6 Column6,
            TColumn7 Column7,
            TColumn8 Column8)>
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

    }
}
