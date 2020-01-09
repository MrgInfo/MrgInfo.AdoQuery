using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core.FormattableStrings;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Run SQL queries.
    /// </summary>
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class SqlProvider
    {
        /// <summary>
        ///     Query with 1 column.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of column.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<T>
        Query<T>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 1)
            select Cast<T>(v[0]);

        /// <summary>
        ///     Query with 2 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2)>
        Query<T1, T2>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 2)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]));

        /// <summary>
        ///     Query with 3 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3)>
        Query<T1, T2, T3>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 3)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));

        /// <summary>
        ///     Query with 4 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>
        Query<T1, T2, T3, T4>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 4)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));

        /// <summary>
        ///     Query with 5 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>
        Query<T1, T2, T3, T4, T5>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 5)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));

        /// <summary>
        ///     Query with 6 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6)>
        Query<T1, T2, T3, T4, T5, T6>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 6)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]));

        /// <summary>
        ///     Query with 7 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7)>
        Query<T1, T2, T3, T4, T5, T6, T7>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 7)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]));

        /// <summary>
        ///     Query with 8 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 8)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]));

        /// <summary>
        ///     Query with 9 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 9)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]));

        /// <summary>
        ///     Query with 10 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 10)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]));

        /// <summary>
        ///     Query with 11 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 11)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]));

        /// <summary>
        ///     Query with 12 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 12)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]));

        /// <summary>
        ///     Query with 13 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <typeparam name="T13">
        ///     Type of column 13.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 13)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]));

        /// <summary>
        ///     Query with 14 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <typeparam name="T13">
        ///     Type of column 13.
        /// </typeparam>
        /// <typeparam name="T14">
        ///     Type of column 14.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 14)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]));

        /// <summary>
        ///     Query with 15 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <typeparam name="T13">
        ///     Type of column 13.
        /// </typeparam>
        /// <typeparam name="T14">
        ///     Type of column 14.
        /// </typeparam>
        /// <typeparam name="T15">
        ///     Type of column 15.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 15)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]));

        /// <summary>
        ///     Query with 16 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <typeparam name="T13">
        ///     Type of column 13.
        /// </typeparam>
        /// <typeparam name="T14">
        ///     Type of column 14.
        /// </typeparam>
        /// <typeparam name="T15">
        ///     Type of column 15.
        /// </typeparam>
        /// <typeparam name="T16">
        ///     Type of column 16.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15, T16 Column16)>
        Query<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FormattableString? sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 16)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]), Cast<T16>(v[15]));

        /// <summary>
        ///     Query with 1 column.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of column.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*' />
        public async IAsyncEnumerable<T>
        QueryAsync<T>(FormattableString? sql,  [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (object?[] row in SafeBetterQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 1, token).ConfigureAwait(false))
            {
                yield return Cast<T>(row[0]);
            }
        }

        public async Task<IEnumerable<(T1 Column1, T2 Column2)>>
        QueryAsync<T1, T2>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 2, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]));

        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3)>>
        QueryAsync<T1, T2, T3>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 3, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]));

        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4)>>
        QueryAsync<T1, T2, T3, T4>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 4, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]));

        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5)>>
        QueryAsync<T1, T2, T3, T4, T5>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 5, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]));

        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6)>>
        QueryAsync<T1, T2, T3, T4, T5, T6>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 6, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]));

        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7)>>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 7, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]));

        /// <summary>
        ///     Query with 16 columns.
        /// </summary>
        /// <typeparam name="T1">
        ///     Type of column 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Type of column 2.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Type of column 3.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Type of column 4.
        /// </typeparam>
        /// <typeparam name="T5">
        ///     Type of column 5.
        /// </typeparam>
        /// <typeparam name="T6">
        ///     Type of column 6.
        /// </typeparam>
        /// <typeparam name="T7">
        ///     Type of column 7.
        /// </typeparam>
        /// <typeparam name="T8">
        ///     Type of column 8.
        /// </typeparam>
        /// <typeparam name="T9">
        ///     Type of column 9.
        /// </typeparam>
        /// <typeparam name="T10">
        ///     Type of column 10.
        /// </typeparam>
        /// <typeparam name="T11">
        ///     Type of column 11.
        /// </typeparam>
        /// <typeparam name="T12">
        ///     Type of column 12.
        /// </typeparam>
        /// <typeparam name="T13">
        ///     Type of column 13.
        /// </typeparam>
        /// <typeparam name="T14">
        ///     Type of column 14.
        /// </typeparam>
        /// <typeparam name="T15">
        ///     Type of column 15.
        /// </typeparam>
        /// <typeparam name="T16">
        ///     Type of column 16.
        /// </typeparam>
        /// <param name="sql">
        ///     The SQL query.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public async Task<IEnumerable<(T1 Column1, T2 Column2, T3 Column3, T4 Column4, T5 Column5, T6 Column6, T7 Column7, T8 Column8, T9 Column9, T10 Column10, T11 Column11, T12 Column12, T13 Column13, T14 Column14, T15 Column15, T16 Column16)>>
        QueryAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FormattableString? sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 16, token).ConfigureAwait(false)
            select (Cast<T1>(v[0]), Cast<T2>(v[1]), Cast<T3>(v[2]), Cast<T4>(v[3]), Cast<T5>(v[4]), Cast<T6>(v[5]), Cast<T7>(v[6]), Cast<T8>(v[7]), Cast<T9>(v[8]), Cast<T10>(v[9]), Cast<T11>(v[10]), Cast<T12>(v[11]), Cast<T13>(v[12]), Cast<T14>(v[13]), Cast<T15>(v[14]), Cast<T16>(v[15]));

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
        ///     Az SQL lekérdezés.
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
            var text = new StringBuilder();
            text.AppendLine()
                .AppendLine(command.CommandText)
                .AppendLine();
            foreach (IDbDataParameter parameter in command.Parameters.OfType<IDbDataParameter>())
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
        ///     Initializes a new instance of <see cref="SqlProvider"/>.
        /// </summary>
        /// <param name="settings">
        ///     Database settings.
        /// </param>
        public SqlProvider(IDatabaseSettings settings) =>
            _settings = settings;

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        DbCommand CreateCommand(DbConnection conncetion, string query, IReadOnlyList<object> parameters)
        {
            DbCommand command = conncetion.CreateCommand();
            try
            {
                var placeholders = new object[parameters.Count];
                for (var i = 0; i < parameters.Count; ++i)
                {
                    var item = new Parameter
                    {
                        Name= _settings.CreateParameterName(i),
                        Value = parameters[i]
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
                command.CommandText = string.Format(null, query, placeholders);
                for (int i = placeholders.Length - 1; i >= 0 ; --i)
                {
                    var placeholder = (Parameter)placeholders[i];
                    DbParameter parameter = command.Parameters[i];
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
        ///      Az SQL lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az SQL lekérdezés helyőrzőkkel.
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
        protected internal virtual IEnumerable<object?[]> Query(string? id, string? format, object[]? args, int columns)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            if (string.IsNullOrEmpty(format)) yield break;
            using DbConnection connection = _settings.CreateConnection();
            connection.Open();
            using DbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                yield return values;
            }
        }

        IEnumerable<object?[]> SafeQuery(string? id, string? format, object[]? args, int columns) =>
            WrapException(id, format, args, () => Query(id, format, args, columns))
            ?? Enumerable.Empty<object[]>();

        protected virtual async IAsyncEnumerable<object?[]> BetterQueryAsync(string? id, string? query, object[]? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(query)) yield break;
            if (token.IsCancellationRequested) yield break;
            await using DbConnection connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using DbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object>());
            await using DbDataReader reader = await (command.ExecuteReaderAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            while (await (reader?.ReadAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false))
            {
                if (token.IsCancellationRequested) break;
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                yield return values;
            }
        }

        async IAsyncEnumerable<object?[]> SafeBetterQueryAsync(string? id, string? query, object[]? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            IAsyncEnumerator<object?[]> enumerator;
            try
            {
                enumerator = BetterQueryAsync(id, query, parameters, columns, token).GetAsyncEnumerator(token);
            }
            catch (DbException exp)
            {
                throw QueryDbException.Create(id, RemoveTrailing(query), parameters, exp);
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
                        throw QueryDbException.Create(id, RemoveTrailing(query), parameters, exp);
                    }
                    if (! hasValue) yield break;
                    yield return enumerator.Current;
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        /// <summary>
        ///     Lekérdezés.
        /// </summary>
        /// <param name="id">
        ///      Az SQL lekérdezés azonosítója.
        /// </param>
        /// <param name="query">
        ///     Az SQL lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="parameters">
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
        protected virtual async Task<IEnumerable<object?[]>> QueryAsync(string? id, string? query, object[]? parameters, int columns, CancellationToken token = default)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            var results = new LinkedList<object[]>();
            if (string.IsNullOrEmpty(query)) return results;
            await using DbConnection connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using DbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object>());
            await using DbDataReader reader = await (command.ExecuteReaderAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            while (await (reader?.ReadAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false))
            {
                if (token.IsCancellationRequested) break;
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                results.AddLast(values);
            }
            return results;
        }

        async Task<IEnumerable<object?[]>> SafeQueryAsync(string? id, string? query, object[]? parameters, int columns, CancellationToken token = default)
        {
            try
            {
                return await QueryAsync(id, query, parameters, columns, token).ConfigureAwait(false);
            }
            catch (DbException exp)
            {
                throw QueryDbException.Create(id, RemoveTrailing(query), parameters, exp);
            }
            
        }

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="id">
        ///      Az SQL lekérdezés azonosítója.
        /// </param>
        /// <param name="query">
        ///     Az SQL lekérdezés helyőrzőkkel.
        /// </param>
        /// <param name="parameters">
        ///     Paraméterek.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        [return: MaybeNull]
        protected internal virtual TResult Read<TResult>(string? id, string? query, object[]? parameters)
        {
            if (string.IsNullOrEmpty(query)) return default!;
            using DbConnection connection = _settings.CreateConnection();
            connection.Open();
            using IDbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object>());
            object result = command.ExecuteScalar();
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
        ///     Az SQL lekérdezés.
        /// </param>
        /// <returns>
        ///     Az adat értéke.
        /// </returns>
        /// <exception cref="DbException">
        ///     Adatbázis hiba!
        /// </exception>
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public TResult Read<TResult>(FormattableString? sql) =>
            SafeRead<TResult>(FindId(sql), sql?.Format, sql?.GetArguments());

        /// <summary>
        ///     Egy konkrét adat lekérdezése.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Az adat típusa.
        /// </typeparam>
        /// <param name="id">
        ///      Az SQL lekérdezés azonosítója.
        /// </param>
        /// <param name="format">
        ///     Az SQL lekérdezés helyőrzőkkel.
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
            await using DbConnection connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using DbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            object result = await (command.ExecuteScalarAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
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
        ///     Az SQL lekérdezés.
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
        /// <include file='Documentation.xml' path='docs/query/*'/>
        public Task<TResult> ReadAsync<TResult>(FormattableString? sql, CancellationToken token = default) =>
            SafeReadAsync<TResult>(FindId(sql), sql?.Format, sql?.GetArguments(), token);

        /// <inheritdoc />
        public override string ToString() => _settings.ToString();
    }
}
