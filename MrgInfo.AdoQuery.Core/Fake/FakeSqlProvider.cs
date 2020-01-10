using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
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

namespace MrgInfo.AdoQuery.Core.Fake
{
    /// <inheritdoc cref="SqlProvider" />
    /// <summary>
    ///     Hamisított adatbázis lekérdezéséket futtató szolgáltatás.
    /// </summary>
    public abstract class FakeSqlProvider: SqlProvider
    {
        sealed class FakeDatabaseSettings: IDatabaseSettings
        {
            public DbConnection CreateConnection() => throw new NotSupportedException();

            public string CreateParameterName(int index) => throw new NotSupportedException();

            public override string ToString() => "Fake database";
        }

        static TraceSource TraceSource { get; } = new TraceSource(nameof(FakeSqlProvider), Information);

        static void Print(SqlQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var text = new StringBuilder();
            text.AppendLine()
                .AppendLine(query.Command)
                .AppendLine();
            foreach (object parameter in query.Parameters)
            {
                text.AppendLine($"\t{parameter}");
            }
            TraceSource.TraceInformation(text.ToString());
        }

        ConcurrentStack<SqlQuery> Queries { get; } = new ConcurrentStack<SqlQuery>();

        /// <inheritdoc />
        protected FakeSqlProvider()
            : base(new FakeDatabaseSettings())
        { }

        /// <summary>
        ///     Lekérdezés hamisítás regisztrációja.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="sql">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="args">
        ///     Az <c>SQL</c> lekérdezés paraméterei.
        /// </param>
        protected void RegisterQuery(string? id, string sql, IEnumerable<object?>? args)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var i = 2;
            MethodBase? method;
            do
            {
                method = new StackFrame(i++).GetMethod();
            }
            while (method?.DeclaringType?.Namespace == "MrgInfo.AdoQuery.Core");
            var sqlQuery = new SqlQuery
            {
                Id = id,
                Caller = $"{method?.DeclaringType?.FullName}.{method?.Name}",
                Command = "\n" + RemoveTrailing(sql) + "\n"
            };
            if (args != null)
            {
                sqlQuery.Parameters.AddRange(args);
            }
            Queries.Push(sqlQuery);
            Print(sqlQuery);
        }

        /// <summary>
        ///     A hamisított eredmény kikeresése.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="query">
        ///     Az <c>SQL</c> lekérdezés.
        /// </param>
        /// <param name="args">
        ///     Az <c>SQL</c> lekérdezés paraméterei.
        /// </param>
        /// <returns>
        ///     A hamisított adatok sor-oszlop indexeléssel.
        /// </returns>
        protected abstract IList<IList<object?>>? FindFakeData(string? id, string? query, IEnumerable<object?>? args);

        internal override TResult Read<TResult>(string? id, string? query, IReadOnlyList<object?>? parameters)
        {
            IList<IList<object?>>? fakeData = FindFakeData(id, query, parameters);
            return fakeData != null && fakeData.Count > 0 && fakeData[0]?.Count > 0
                ? Cast<TResult>(fakeData[0][0])
                : default;
        }

        private protected override Task<TResult> ReadAsync<TResult>(string? id, string? format, IReadOnlyList<object?>? args, CancellationToken token = default) =>
            Task.Run(() => Read<TResult>(id, format, args), token);

        internal override IEnumerable<object?[]> Query(string? id, string? query, IReadOnlyList<object?>? parameters, int columns)
        {
            IList<IList<object?>>? fakeData = FindFakeData(id, query, parameters);
            if (fakeData == null) yield break;
            foreach (IList<object?> row in fakeData.Where(_ => _ != null))
            {
                yield return row.ToArray();
            }
        }

        private protected override async IAsyncEnumerable<object?[]> QueryAsync(string? id, string? query, IReadOnlyList<object?>? parameters, int columns, [EnumeratorCancellation] CancellationToken token = default)
        {
            await Task.Yield(); // TODO
            foreach (object?[] row in Query(id, query, parameters, columns))
            {
                if (token.IsCancellationRequested) yield break;
                yield return row;
            }
        }

        /// <summary>
        ///     Összegyűjtött lekérdezése mentése.
        /// </summary>
        /// <param name="writer">
        ///     A kiírt adatfolyam.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="writer"/> értéke <c>null</c>.
        /// </exception>
        public void SaveAllQueries(TextWriter writer)
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
            var data = new SqlQueriesCollection(Queries);
            var dcs = new DataContractSerializer(data.GetType());
            dcs.WriteObject(dictionaryWriter, data);
        }

        /// <summary>
        ///     Összegyűjtött lekérdezése mentése.
        /// </summary>
        /// <param name="fileName">
        ///     A fájl neve.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="fileName"/> értéke <c>null</c>.
        /// </exception>
        public void SaveAllQueries(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            if (Queries.IsEmpty) return;
            using StreamWriter stream = File.CreateText(fileName);
            SaveAllQueries(stream);
        }
    }
}
