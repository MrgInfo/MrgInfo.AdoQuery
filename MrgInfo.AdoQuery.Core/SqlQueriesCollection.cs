using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc />
    [CollectionDataContract(Name = "Queries", Namespace = "", ItemName = "Query")]
    public sealed class SqlQueriesCollection: List<SqlQuery>
    {
        /// <inheritdoc />
        public SqlQueriesCollection()
        { }

        /// <inheritdoc />
        public SqlQueriesCollection(IEnumerable<SqlQuery> queries)
            : base(queries)
        { }
    }
}
