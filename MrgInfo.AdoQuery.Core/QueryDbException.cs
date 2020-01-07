using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;

namespace Sda.Query
{
    /// <inheritdoc />
    [Serializable]
    public sealed class QueryDbException: DbException
    {
        const string _message = "Database error!";

        /// <inheritdoc />
        public QueryDbException()
            : base(_message)
        { }

        /// <inheritdoc />
        public QueryDbException(string message)
            : base(message ?? _message)
        { }

        /// <inheritdoc />
        public QueryDbException(string message, Exception inner)
            : base(message ?? _message, inner)
        { }

        QueryDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        internal static QueryDbException Create(string? id, string command, IEnumerable<object>? parameters, Exception exp)
        {
            string message = $@"
#ID
{id}

#QUERY
{command}

#PARAMETERS
{string.Join(",", parameters?.Where(_ => _ != null).Select(_ => _.ToString()) ?? Enumerable.Empty<string>())}

#ERROR
{exp}
";
            return new QueryDbException(message, exp);
        }
    }
}
