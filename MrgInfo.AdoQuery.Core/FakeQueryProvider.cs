using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        ///     TODO Lekérdezés hamisítás regisztrációja.
        /// </summary>
        /// <param name="id">
        ///     TODO Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="sql">
        ///     TODO Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="args">
        ///     TODO Az <c>SQL</c> lekérdezés paraméterei.
        /// </param>
        protected void RegisterQuery(string? id, string sql, IEnumerable<object?>? args)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

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
                Command = $"\n{RemoveTrailing(sql)}\n"
            };
            if (args != null)
            {
                sqlQuery.Parameters.AddRange(args);
            }
            Queries.Push(sqlQuery);
            Print(sqlQuery);
        }

        /// <summary>
        ///     TODO A hamisított eredmény kikeresése.
        /// </summary>
        /// <param name="id">
        ///     TODO Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="query">
        ///     TODO Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="args">
        ///     TODO Az <c>SQL</c> lekérdezés paraméterei.
        /// </param>
        /// <returns>
        ///     TODO A hamisított adatok sor-oszlop indexeléssel.
        /// </returns>
        protected abstract IList<IReadOnlyList<object?>> FindFakeData(string? id, string? query, IEnumerable<object?>? args);

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
        ///     No real async.
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
        protected internal override TResult Read<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters)
        {
            IList<IReadOnlyList<object?>>? fakeData = FindFakeData(id, query, parameters);
            return fakeData != null && fakeData.Count > 0 && fakeData[0]?.Count > 0
                ? Cast<TResult>(fakeData[0][0])
                : default;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     No real async.
        /// </remarks>
        protected internal override async Task<TResult> ReadAsync<TResult>(string? id, string? query, IReadOnlyList<object?>? args, CancellationToken token = default) =>
            await Task.FromResult(Read<TResult>(id, query, args)).ConfigureAwait(false);

        /// <summary>
        ///     TODO Összegyűjtött lekérdezése mentése.
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
        ///     Save registered queries.
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
