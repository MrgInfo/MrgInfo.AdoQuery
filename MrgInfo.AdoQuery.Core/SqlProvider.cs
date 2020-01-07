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
using static System.Linq.Enumerable;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Sda.Query
{
    /// <summary>
    ///     Adatbázis lekérdezéséket futtató szolgáltatás.
    /// </summary>
    public partial class SqlProvider
    {
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

        static TraceSource TraceSource { get; }  = new TraceSource(nameof(SqlProvider), SourceLevels.Information);

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
        protected static TResult WrapException<TResult>(string id, string? query, IEnumerable<object>? parameters, Func<TResult> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            try
            {
                return function();
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
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected static async Task<TResult> WrapExceptionAsync<TResult>(string? query, string? id, IEnumerable<object>? parameters, Func<Task<TResult>> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            try
            {
                return await (function() ?? FromResult(default(TResult))).ConfigureAwait(false);
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
                result.Append(shortLine.StartsWith("|", OrdinalIgnoreCase)
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
        protected static TResult Cast<TResult>(object value)
        {
            Type type = typeof(TResult);
            if (type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
            }

            if (value is null || value is DBNull)
            {
                return default;
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

        static string? FindId(FormattableString formattableString) =>
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

            DbCommand command = conncetion.CreateCommand();
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
                    Debug.Assert(placeholder != null, nameof(placeholder) + " != null");
                    DbParameter parameter = command.Parameters[i];
                    Debug.Assert(parameter != null, nameof(parameter) + " != null");
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
        protected internal virtual IEnumerable<object[]> Query(string id, string format, object[] args, int columns)
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

        IEnumerable<object[]> SafeQuery(string id, string? format, object[]? args, int columns) =>
            WrapException(id, format, args, () => Query(id, format, args, columns))
            ?? Empty<object[]>();

        /// <summary>
        ///     Lekérdezés 1 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     Az oszlop típusa.
        /// </typeparam>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <returns>
        ///     Találati lista.
        /// </returns>
        /// <include file='Documentation.xml' path='docs/sqlformat/*'/>
        public IEnumerable<TColumn> Query<TColumn>(FormattableString sql) =>
            from v in SafeQuery(FindId(sql), sql?.Format, sql?.GetArguments(), 1)
            select Cast<TColumn>(v[0]);

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
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected virtual async Task<IEnumerable<object[]>> QueryAsync(string id, string format, object[] args, int columns, CancellationToken token)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            var results = new LinkedList<object[]>();
            if (string.IsNullOrEmpty(format)) return results;
            using (DbConnection connection = _settings.CreateConnection())
            {
                await (connection.OpenAsync(token) ?? CompletedTask).ConfigureAwait(false);
                using DbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
                using DbDataReader reader = await (command.ExecuteReaderAsync(token) ?? FromResult<DbDataReader>(null)).ConfigureAwait(false);
                while (await (reader?.ReadAsync(token) ?? FromResult(false)).ConfigureAwait(false))
                {
                    if (token.IsCancellationRequested) break;
                    var values = new object[columns];
                    if ((reader?.GetValues(values) ?? 0) == 0) continue;
                    results.AddLast(values);
                }
            }
            return results;
        }

        Task<IEnumerable<object[]>> SafeQueryAsync(string id, string format, object[] args, int columns, CancellationToken token) =>
            WrapExceptionAsync(id, format, args, async () => await QueryAsync(id, format, args, columns, token).ConfigureAwait(false));

        /// <summary>
        ///     Lekérdezés 1 oszloppal.
        /// </summary>
        /// <typeparam name="TColumn">
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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public async Task<IEnumerable<TColumn>> QueryAsync<TColumn>(FormattableString sql, CancellationToken token = default) =>
            from v in await SafeQueryAsync(FindId(sql), sql?.Format, sql?.GetArguments(), 1, token).ConfigureAwait(false)
            select Cast<TColumn>(v[0]);

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
        protected internal virtual TResult Read<TResult>(string? id, string? format, object[]? args)
        {
            if (string.IsNullOrEmpty(format)) return default;
            using DbConnection connection = _settings.CreateConnection();
            connection.Open();
            using IDbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            object result = command.ExecuteScalar();
            if (result is null || result is DBNull) return default;
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
        public TResult Read<TResult>(FormattableString sql) =>
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
        public bool TryRead<TResult>(FormattableString sql, out TResult result)
        {
            try
            {
                result = Read<TResult>(FindId(sql), sql?.Format, sql?.GetArguments());
                return true;
            }
            catch(DbException)
            {
                result = default;
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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected internal virtual async Task<TResult> ReadAsync<TResult>(string id, string format, object[] args, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(format)) return default;
            using DbConnection connection = _settings.CreateConnection();
            await (connection.OpenAsync(token) ?? CompletedTask).ConfigureAwait(false);
            using DbCommand command = CreateCommand(connection, RemoveTrailing(format), args ?? Array.Empty<object>());
            object result = await (command.ExecuteScalarAsync(token) ?? FromResult<object>(null)).ConfigureAwait(false);
            if (result is null || result is DBNull)
                return default;
            return (TResult)Convert.ChangeType(result, typeof(TResult));
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
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public Task<TResult> ReadAsync<TResult>(FormattableString sql, CancellationToken token = default) =>
            SafeReadAsync<TResult>(FindId(sql), sql?.Format, sql?.GetArguments(), token);

        /// <inheritdoc />
        public override string ToString() => _settings.ToString();
    }
}
