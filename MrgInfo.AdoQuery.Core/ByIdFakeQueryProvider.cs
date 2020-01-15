using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc cref="FakeQueryProvider" />
    /// <summary>
    ///     Fake data provider by unique identifier.
    /// </summary>
    public sealed class ByIdFakeQueryProvider: FakeQueryProvider
    {
        ConcurrentDictionary<string, IList<IReadOnlyList<object?>>> ByIdData { get; } = new ConcurrentDictionary<string, IList<IReadOnlyList<object?>>>();

        /// <inheritdoc />
        protected override IList<IReadOnlyList<object?>> FindFakeData(string? id, string? query, IEnumerable<object?>? args)
        {
            object[] parameters =
                args
                ?.Select((a, i) => (object)new Parameter { Name = $"{{{i}}}", Value = a })
                .ToArray()
                ?? Array.Empty<object>();
            string command = string.Format(null, query ?? "", parameters);
            RegisterQuery(id, command, from Parameter p in parameters select p.Value);
            if (! string.IsNullOrWhiteSpace(id)
                && ByIdData.TryGetValue(id, out IList<IReadOnlyList<object?>>? data))
            {
                return data;
            }
            return Array.Empty<IReadOnlyList<object?>>();
        }

        /// <summary>
        ///     Register fake data for a query.
        /// </summary>
        /// <param name="id">
        ///     Unique identifier of the query.
        /// </param>
        /// <param name="data">
        ///     Fake result set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="data"/> argument has <c>null</c> value.
        /// </exception>
        public void Register<TId>(TId id, params IReadOnlyList<object?>[] data) where TId: struct =>
            ByIdData.TryAdd($"{id}", data ?? throw new ArgumentNullException(nameof(data)));

        /// <summary>
        ///     Register fake data for a given query.
        /// </summary>
        /// <param name="member">
        ///     Caller context of the query.
        /// </param>
        /// <param name="id">
        ///     Unique identifier of the query.
        /// </param>
        /// <param name="data">
        ///     Fake result set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="member"/> or <paramref name="data"/> argument has <c>null</c> value.
        /// </exception>
        public void Register<TId>(string member, TId id, params IReadOnlyList<object?>[] data) where TId: struct =>
            ByIdData.TryAdd($"{member ?? throw new ArgumentNullException(nameof(member))}/{id}", data ?? throw new ArgumentNullException(nameof(data)));

        /// <summary>
        ///     Register fake data for a given query.
        /// </summary>
        /// <param name="id">
        ///     Unique identifier of the query.
        /// </param>
        /// <value>
        ///     Fake result set.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="id"/> or <paramref name="value"/> argument has <c>null</c> value.
        /// </exception>
        public IList<IReadOnlyList<object?>> this[string id]
        {
            get => (ByIdData.TryGetValue(id ?? throw new ArgumentNullException(nameof(id)), out IList<IReadOnlyList<object?>> value) ? value : null)
                   ?? Array.Empty<IReadOnlyList<object?>>();
            set => ByIdData.TryAdd(id ?? throw new ArgumentNullException(nameof(id)), value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        ///     Register fake data for a given query.
        /// </summary>
        /// <param name="member">
        ///     Caller context of the query.
        /// </param>
        /// <param name="id">
        ///     Unique identifier of the query.
        /// </param>
        /// <value>
        ///     Fake result set.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="member"/> or <paramref name="id"/> or <paramref name="value"/> argument has <c>null</c> value.
        /// </exception>
        public IList<IReadOnlyList<object?>> this[string member, string id]
        {
            get => (ByIdData.TryGetValue($"{member}/{id}", out IList<IReadOnlyList<object?>> value) ? value : null)
                   ?? Array.Empty<IReadOnlyList<object?>>();
            set => ByIdData.TryAdd($"{member ?? throw new ArgumentNullException(nameof(member))}/{id ?? throw new ArgumentNullException(nameof(id))}", value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        ///     Clear registered data.
        /// </summary>
        public void Clear() => ByIdData.Clear();
    }
}
