using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static System.Globalization.CultureInfo;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Egy <c>SQL</c> lekérdezés futtatási körülményeit leíró osztály.
    /// </summary>
    [DataContract(Name = "Query", Namespace = "")]
    public sealed class Query
    {
        /// <summary>
        ///     Az delegált segítségével határozhatjuk meg, hogyan cseréljük a fiktív
        ///     adatbázis azonosítókat valós azonosítókra.
        /// </summary>
        public static Func<int, int>? IdMapper { get; set; }

        static object?[] MapIds(IReadOnlyList<object?> parameters)
        {
            if (parameters == null) return Array.Empty<object>();
            var result = new List<object?>();
            foreach (object? parameter in parameters)
            {
                if (parameter is int id)
                {
                    result.Add(IdMapper?.Invoke(id) ?? id);
                }
                else
                {
                    result.Add(parameter);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [DataMember(Order = 0)]
        public string? Id { get; set; }

        /// <summary>
        ///     Caller method.
        /// </summary>
        [DataMember(Order = 1)]
        public string? Caller { get; set; }

        /// <summary>
        ///     The query command.
        /// </summary>
        [DataMember(Order = 2)]
        public string? Command { get; set; }

        /// <summary>
        ///     Query parameters.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<object?> Parameters { get; private set; } = new List<object?>();

        /// <summary>
        ///     Serializing query parameters.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="value"/> has <c>null</c> value.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Invalid format.
        /// </exception>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [DataMember(Order = 3, Name =nameof(Parameters))]
        public QueryParameter[] QueryParameters
        {
            get
            {
                var result = new QueryParameter[Parameters.Count];
                var i = 0;
                foreach (object? par in Parameters)
                {
                    result[i++] = par == null
                        ? new QueryParameter()
                        : new QueryParameter
                        {
                            TypeName = par.GetType().FullName,
                            Value = string.Format(InvariantCulture, "{0}", par)
                        };
                }
                return result;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                Parameters = (
                    from par in value
                    let type = par?.TypeName == null
                        ? null
                        : Type.GetType(par.TypeName, false)
                    select type == null
                        ? null
                        : Convert.ChangeType(par.Value, type, InvariantCulture))
                    .ToList();
            }
        }

        /// <summary>
        ///     Run query.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="provider"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        public void Run(DbQueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            object?[] args = MapIds(Parameters.ToArray());
            try
            {
                provider.Read<string>(Id, Command ?? "", args);
            }
            catch (Exception exp)
            {
                throw new QueryDbException(Id, Command, Parameters, exp);
            }
        }

        /// <summary>
        ///     Run query asynchronously.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="provider"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        public async Task RunAsync(DbQueryProvider provider, CancellationToken token = default)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            object?[] args = MapIds(Parameters.ToArray());
            try
            {
                await provider.ReadAsync<string>(Id, Command ?? "", args, token).ConfigureAwait(false);
            }
            catch (Exception exp)
            {
                throw new QueryDbException(Id, Command, Parameters, exp);
            }
        }

        /// <summary>
        ///     Run query and count records.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        /// <returns>
        ///     Record count.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="provider"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        public int? RunCount(DbQueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            object?[] args = MapIds(Parameters.ToArray());
            try
            {
                return provider.Query(Id, Command, args, 1).Count();
            }
            catch (Exception exp)
            {
                throw new QueryDbException(Id, Command, Parameters, exp);
            }
        }

        /// <summary>
        ///     Run query asynchronously.
        /// </summary>
        /// <param name="provider">
        ///     Query provider.
        /// </param>
        /// <param name="token">
        ///     The cancellation token that will be checked for stop reading.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="provider"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Error occured during executing the query.
        /// </exception>
        public async Task<int?> RunCountAsync(DbQueryProvider provider, CancellationToken token = default)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            object?[] args = MapIds(Parameters.ToArray());
            try
            {
                var i = 0;
                await foreach (object?[] _ in provider.QueryAsync(Id, Command ?? "", args, 1, token))
                {
                    ++i;
                }
                return i;
            }
            catch (Exception exp)
            {
                throw new QueryDbException(Id, Command, Parameters, exp);
            }
        }
    }
}
