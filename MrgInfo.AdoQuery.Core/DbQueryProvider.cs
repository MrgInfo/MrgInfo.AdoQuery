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

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Run SQL queries on database.
    /// </summary>
    public abstract class DbQueryProvider: QueryProvider
    {
        static TraceSource TraceSource { get; } = new TraceSource(nameof(DbQueryProvider), SourceLevels.Information);

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
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        protected static string GetParameterNumber(int index)
        {
            if (index < 0 || 98 < index) throw new ArgumentOutOfRangeException(nameof(index), index, "0-98");
            return $"{index + 1:00}";
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        DbCommand CreateCommand(DbConnection conncetion, string query, IReadOnlyList<object?> parameters)
        {
            DbCommand command = conncetion.CreateCommand();
            try
            {
                var placeholders = new object[parameters.Count];
                for (var i = 0; i < parameters.Count; ++i)
                {
                    var item = new Parameter
                    {
                        Name = CreateParameterName(i),
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
                for (int i = placeholders.Length - 1; i >= 0; --i)
                {
                    var placeholder = (Parameter) placeholders[i];
                    DbParameter parameter = command.Parameters[i];
                    parameter.Value = placeholder.Value ?? DBNull.Value;
                    if (!placeholder.Present) command.Parameters.Remove(parameter);
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
        ///     Create nem database connection.
        /// </summary>
        /// <returns>
        ///     New connection.
        /// </returns>
        /// <exception cref="DbException">
        ///     Cannot connect to database.
        /// </exception>
        protected abstract DbConnection CreateConnection();

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
        protected abstract string CreateParameterName(int index);

        /// <inheritdoc />
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        protected internal override IEnumerable<object?[]> Query(string? id, string? query, IReadOnlyList<object?>? parameters, int columns)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            if (string.IsNullOrEmpty(query)) yield break;
            using DbConnection connection = CreateConnection();
            connection.Open();
            using DbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object?>());
            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var values = new object[columns];
                if (reader.GetValues(values) == 0) continue;
                yield return values;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        protected internal override async IAsyncEnumerable<object?[]> QueryAsync(string? id, string? query, IReadOnlyList<object?>? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns), columns, "> 0!");

            if (string.IsNullOrEmpty(query)) yield break;
            if (token.IsCancellationRequested) yield break;
            await using DbConnection connection = CreateConnection();
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

        /// <inheritdoc />
        [return: MaybeNull]
        protected internal override TResult Read<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters)
        {
            if (string.IsNullOrEmpty(query)) return default!;
            using DbConnection connection = CreateConnection();
            connection.Open();
            using IDbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object>());
            object result = command.ExecuteScalar();
            if (result is null || result is DBNull) return default!;
            return (TResult) Convert.ChangeType(result, typeof(TResult), CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        protected internal override async Task<TResult> ReadAsync<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(query)) return default!;
            await using DbConnection connection = CreateConnection();
            await (connection.OpenAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            await using DbCommand command = CreateCommand(connection, RemoveTrailing(query), parameters ?? Array.Empty<object>());
            object result = await (command.ExecuteScalarAsync(token) ?? throw new InvalidOperationException()).ConfigureAwait(false);
            if (result is null || result is DBNull) return default!;
            return (TResult)Convert.ChangeType(result, typeof(TResult), CultureInfo.InvariantCulture);
        }
    }
}