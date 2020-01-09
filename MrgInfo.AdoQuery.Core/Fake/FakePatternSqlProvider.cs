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
        ConcurrentDictionary<Regex, IList<IList<object?>>?> ByRegexData { get; } = new ConcurrentDictionary<Regex, IList<IList<object?>>?>();

        /// <inheritdoc />
        protected override IList<IList<object?>>? FindFakeData(string? id, string? format, IEnumerable<object>? args)
        {
            object[] parameters =
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
                ?? Array.Empty<IList<object?>>();
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
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public void Add(Regex pattern, params object?[][]? data) =>
            ByRegexData.TryAdd(pattern, data);

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="pattern">
        ///     Az <c>SQL</c> kifejezés mintaillesztése.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public void Add(string pattern, params object?[][]? data) =>
            ByRegexData.TryAdd(new Regex(pattern, Compiled | IgnoreCase | Singleline | CultureInvariant), data);

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="pattern">
        ///     Az <c>SQL</c> lekérdezés mintaillesztése.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        public IList<IList<object?>>? this[string pattern]
        {
            get => ByRegexData.TryGetValue(new Regex(pattern ?? "^$", Compiled | IgnoreCase | Singleline | CultureInvariant), out IList<IList<object?>>? value)
                ? value
                : null;
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
