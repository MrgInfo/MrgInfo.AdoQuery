using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc />
    [CollectionDataContract(Name = "Queries", Namespace = "", ItemName = "Query")]
    public sealed class QueriesCollection: List<Query>
    {
        /// <summary>
        ///     Load query collection from <c>XML</c> file.
        /// </summary>
        /// <param name="fileName">
        ///     Import file's name.
        /// </param>
        /// <returns>
        ///     Query collection.
        /// </returns>
        public static QueriesCollection Load(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            using StreamReader reader = File.OpenText(fileName);
            return Load(reader);
        }

        /// <summary>
        ///     Load query collection from <c>XML</c> file.
        /// </summary>
        /// <param name="reader">
        ///     File stream.
        /// </param>
        /// <returns>
        ///     Query collection.
        /// </returns>
        public static QueriesCollection Load(StreamReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var settings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Document,
                Async = true,
                CheckCharacters = true,
                CloseInput = true,
                IgnoreComments = true,
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreProcessingInstructions = true,
                MaxCharactersFromEntities = long.MaxValue,
            };
            using var dictionaryReader = XmlReader.Create(reader, settings);
            var data = new QueriesCollection();
            var dcs = new DataContractSerializer(data.GetType());
            return dcs.ReadObject(dictionaryReader) as QueriesCollection
                   ?? new QueriesCollection();
        }

        /// <inheritdoc />
        public QueriesCollection()
        { }

        /// <inheritdoc />
        public QueriesCollection(IEnumerable<Query> queries)
            : base(queries)
        { }
    }
}
