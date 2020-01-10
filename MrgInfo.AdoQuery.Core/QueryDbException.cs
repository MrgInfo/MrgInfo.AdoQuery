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

        QueryDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        internal static QueryDbException Create(string? id, string? query, IEnumerable<object?>? parameters, Exception exp)
        {
            string message = $@"
#ID
{id}

#QUERY
{query}

#PARAMETERS
{string.Join(",", parameters?.Where(_ => _ != null).Select(_ => _.ToString()) ?? Enumerable.Empty<string>())}

#ERROR
{exp}
";
            return new QueryDbException(message, exp);
        }
    }
}
