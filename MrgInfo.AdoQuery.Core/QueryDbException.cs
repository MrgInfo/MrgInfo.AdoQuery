using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc />
    [Serializable]
    public sealed class QueryDbException: DbException
    {
        const string DefaultMessage = "Database error!";

        static string CreateMessage(string? id, string? query, IEnumerable<object?>? parameters, Exception exp) => $@"
#ID
{id}

#QUERY
{query}

#PARAMETERS
{string.Join(",", parameters?.Select(_ => _?.ToString()) ?? Enumerable.Empty<string>())}

#ERROR
{exp}
";

        /// <inheritdoc />
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        public QueryDbException()
            : base(DefaultMessage)
        { }

        /// <inheritdoc />
        public QueryDbException(string message)
            : base(message ?? DefaultMessage)
        { }

        /// <inheritdoc />
        public QueryDbException(string message, Exception inner)
            : base(message ?? DefaultMessage, inner)
        { }

        internal QueryDbException(string? id, string? query, IEnumerable<object?>? parameters, Exception inner)
            : base(CreateMessage(id, query, parameters, inner), inner)
        { }

        QueryDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
