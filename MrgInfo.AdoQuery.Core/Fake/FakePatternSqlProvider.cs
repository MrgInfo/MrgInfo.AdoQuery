using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;

namespace MrgInfo.AdoQuery.Core.Fake
{
    /// <summary>
    ///     Reguláris kifejezések segítségével hamisított lekérdezések.
    /// </summary>
    /// <inheritdoc cref="FakeSqlProvider" />
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public sealed class FakePatternSqlProvider: FakeSqlProvider, IEnumerable
    {
        ConcurrentDictionary<Regex, object[][]?> ByRegexData { get; } = new ConcurrentDictionary<Regex, object[][]?>();

        /// <inheritdoc />
        protected override object[][] FindFakeData(string? id, string? format, IEnumerable<object>? args)
        {
            var parameters =
                args
                ?.Select((a, i) => (object)new Parameter { Name = $"{{{i}}}", Value = a })
                .ToArray()
                ?? Array.Empty<object>();
            string command = string.Format(null, format ?? "", parameters);
            RegisterQuery(id, command, from Parameter p in parameters where p != null select p.Value);
            return (
                from re in ByRegexData
                where re.Key != null && re.Value != null && re.Key.IsMatch(command)
                select re.Value)
                .FirstOrDefault()
                ?? Array.Empty<object[]>();
        }

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="pattern">
        ///     Az <c>SQL</c> kifejezés mintaillesztése.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="pattern"/> vagy a <paramref name="data"/> értéke <c>null</c>.
        /// </exception>
        public void Add(Regex pattern, params object[][] data)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ByRegexData.TryAdd(pattern, data);
        }

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="pattern">
        ///     Az <c>SQL</c> kifejezés mintaillesztése.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="pattern"/> vagy a <paramref name="data"/> értéke <c>null</c>.
        /// </exception>
        public void Add(string pattern, params object[][] data)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ByRegexData.TryAdd(new Regex(pattern, Compiled | IgnoreCase | Singleline | CultureInvariant), data);
        }

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="pattern">
        ///     Az <c>SQL</c> lekérdezés mintaillesztése.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[][]? this[string pattern]
        {
            get => ByRegexData.TryGetValue(new Regex(pattern ?? "^$", Compiled | IgnoreCase | Singleline | CultureInvariant), out var value) ? value : null;
            set => ByRegexData.TryAdd(new Regex(pattern ?? "^$", Compiled | IgnoreCase | Singleline | CultureInvariant), value);
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator() => ByRegexData.GetEnumerator();

        /// <summary>
        ///     A hamisított lekérdezési eredmények törlése.
        /// </summary>
        public void Clear() => ByRegexData.Clear();
    }
}
