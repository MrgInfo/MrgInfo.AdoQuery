using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static System.Diagnostics.SourceLevels;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc cref="QueryProvider" />
    /// <summary>
    ///     Fake data query provider.
    /// </summary>
    public abstract class FakeQueryProvider: QueryProvider
    {
        static TraceSource TraceSource { get; } = new TraceSource(nameof(FakeQueryProvider), Information);

        static void Print(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var text = new StringBuilder();
            text.AppendLine()
                .AppendLine(RemoveTrailing(query.Command))
                .AppendLine();
            foreach (object? parameter in query.Parameters)
            {
                text.AppendLine($"\t{parameter ?? "NULL"}");
            }
            TraceSource.TraceInformation(text.ToString());
        }

        ConcurrentStack<Query> Queries { get; } = new ConcurrentStack<Query>();

        /// <summary>
        ///     Used for collection queries in order to test them on a particular database for
        ///     syntax and performance.
        /// </summary>
        /// <param name="id">
        ///     The unique identifier of query.
        /// </param>
        /// <param name="query">
        ///     The query command.
        /// </param>
        /// <param name="parameters">
        ///      Query parameters.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="query"/> argument has <c>null</c> value.
        /// </exception>

        protected void RegisterQuery(string? id, string query, IEnumerable<object?>? parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            string @namespace = typeof(QueryProvider).Namespace ?? "@";
            var i = 2;
            MethodBase? method;
            do
            {
                method = new StackFrame(i++).GetMethod();
            }
            while (method.DeclaringType?.Namespace != null 
                   && method.DeclaringType.Namespace.StartsWith(@namespace, StringComparison.Ordinal));
            var sqlQuery = new Query
            {
                Id = id,
                Caller = $"{method.DeclaringType?.FullName}.{method.Name}",
                Command = $"\n{RemoveTrailing(query)}\n"
            };
            if (parameters != null)
            {
                sqlQuery.Parameters.AddRange(parameters);
            }
            Queries.Push(sqlQuery);
            Print(sqlQuery);
        }

        /// <summary>
        ///     Load fake data.
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
        /// <returns>
        ///     Fake data with [row][column] indexing.
        /// </returns>
        protected abstract IList<IReadOnlyList<object?>> FindFakeData(string? id, string? query, IEnumerable<object?>? parameters);

        /// <inheritdoc />
        protected internal override IEnumerable<object?[]> Query(string? id, string? query, IReadOnlyList<object?>? parameters, int columns)
        {
            IList<IReadOnlyList<object?>>? fakeData = FindFakeData(id, query, parameters);
            if (fakeData == null) yield break;
            foreach (IReadOnlyList<object?> row in fakeData)
            {
                yield return row.ToArray();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        ///     Not really async!
        /// </remarks>
        protected internal override async IAsyncEnumerable<object?[]> QueryAsync(string? id, string? query, IReadOnlyList<object?>? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            await Task.Yield();
            foreach (object?[] row in Query(id, query, parameters, columns))
            {
                if (token.IsCancellationRequested) yield break;
                yield return row;
            }
        }

        /// <inheritdoc />
        [return: MaybeNull]
        protected internal override TResult Read<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters)
        {
            IList<IReadOnlyList<object?>>? fakeData = FindFakeData(id, query, parameters);
            return fakeData != null && fakeData.Count > 0 && fakeData[0]?.Count > 0
                ? Cast<TResult>(fakeData[0][0])
                : default;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     Not really async!
        /// </remarks>
        protected internal override async Task<TResult> ReadAsync<TResult>(string? id, string? query, IReadOnlyList<object?>? args, CancellationToken token = default)
        {
            var fake = Read<TResult>(id, query, args);
            return fake is null
                ? default
                : await Task.FromResult(fake).ConfigureAwait(false);
        }

        /// <summary>
        ///     Export collected queries to <c>XML</c> file.
        /// </summary>
        /// <param name="writer">
        ///     File stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="writer" /> argument has <c>null</c> value.
        /// </exception>
        public void SaveQueries(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (Queries.IsEmpty) return;
            var settings = new XmlWriterSettings
            {
                Encoding = writer.Encoding,
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                ConformanceLevel = ConformanceLevel.Document,
                WriteEndDocumentOnClose = true,
                CloseOutput = false,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Async = true,
                CheckCharacters = true,
                DoNotEscapeUriAttributes = true,
                NewLineOnAttributes = false,
                OmitXmlDeclaration = false
            };
            using var dictionaryWriter = XmlWriter.Create(writer, settings);
            dictionaryWriter.WriteStartDocument();
            var data = new QueriesCollection(Queries);
            var dcs = new DataContractSerializer(data.GetType());
            dcs.WriteObject(dictionaryWriter, data);
        }

        /// <summary>
        ///     Export collected queries to <c>XML</c> file.
        /// </summary>
        /// <param name="fileName">
        ///     Export file's name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="fileName" /> argument has <c>null</c> value.
        /// </exception>
        public void SaveQueries(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            if (Queries.IsEmpty) return;
            using StreamWriter stream = File.CreateText(fileName);
            SaveQueries(stream);
        }

        /// <inheritdoc />
        public override string ToString() => "Fake database";
    }
}
