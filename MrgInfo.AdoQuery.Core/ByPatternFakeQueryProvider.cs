using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc cref="FakeQueryProvider" />
    /// <summary>
    ///     Fake data provider by regular expression query matching.
    /// </summary>
    public sealed class ByPatternFakeQueryProvider: FakeQueryProvider
    {
        ConcurrentDictionary<Regex, IList<IReadOnlyList<object?>>> ByRegexData { get; } = new ConcurrentDictionary<Regex, IList<IReadOnlyList<object?>>>();

        /// <inheritdoc />
        protected override IList<IReadOnlyList<object?>> FindFakeData(string? id, string? query, IEnumerable<object?>? parameters)
        {
            object[] formatParameters =
                parameters
                ?.Select((a, i) => (object)new Parameter { Name = $"{{{i}}}", Value = a })
                .ToArray()
                ?? Array.Empty<object>();
            string command = string.Format(null, query ?? "", formatParameters);
            RegisterQuery(id, command, from Parameter p in formatParameters select p.Value);
            return (
                from re in ByRegexData
                where re.Key != null && re.Value != null && re.Key.IsMatch(command)
                select re.Value)
                .FirstOrDefault()
                ?? Array.Empty<IReadOnlyList<object?>>();
        }

        /// <summary>
        ///     Register fake data for a query.
        /// </summary>
        /// <param name="pattern">
        ///     Regular expression for selecting query.
        /// </param>
        /// <param name="data">
        ///     Fake result set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="pattern"/> or <paramref name="data"/> argument has <c>null</c> value.
        /// </exception>
        public void Register(Regex pattern, params IReadOnlyList<object?>[] data) =>
            ByRegexData.TryAdd(
                pattern ?? throw new ArgumentNullException(nameof(pattern)),
                data ?? throw new ArgumentNullException(nameof(data)));

        /// <summary>
        ///     Register fake data for a query.
        /// </summary>
        /// <param name="pattern">
        ///     Regular expression for selecting query.
        /// </param>
        /// <param name="data">
        ///     Fake result set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="pattern"/> or <paramref name="data"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     A regular expression parsing error occurred.
        /// </exception>
        public void Register(string pattern, params IReadOnlyList<object?>[] data) =>
            ByRegexData.TryAdd(
                new Regex(pattern ?? throw new ArgumentNullException(nameof(pattern)), Compiled | IgnoreCase | Singleline | CultureInvariant),
                data ?? throw new ArgumentNullException(nameof(data)));

        /// <summary>
        ///     Register fake data for a given query.
        /// </summary>
        /// <param name="pattern">
        ///     Unique identifier of the query.
        /// </param>
        /// <value>
        ///     Fake result set.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="pattern"/> or <paramref name="value"/> argument has <c>null</c> value.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     A regular expression parsing error occurred.
        /// </exception>
        public IList<IReadOnlyList<object?>> this[string pattern]
        {
            get => ByRegexData.TryGetValue(
                new Regex(pattern ?? throw new ArgumentNullException(nameof(pattern)), Compiled | IgnoreCase | Singleline | CultureInvariant),
                out IList<IReadOnlyList<object?>> value)
                    ? value
                    : Array.Empty<IReadOnlyList<object?>>();
            set => ByRegexData.TryAdd(
                new Regex(pattern ?? throw new ArgumentNullException(nameof(pattern)), Compiled | IgnoreCase | Singleline | CultureInvariant),
                value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        ///     Clear registered data.
        /// </summary>
        public void Clear() => ByRegexData.Clear();
    }
}
