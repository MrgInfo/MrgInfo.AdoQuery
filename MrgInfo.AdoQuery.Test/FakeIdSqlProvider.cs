using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sda.Query
{
    /// <summary>
    ///     Egyedi azonosítók segítségével hamisított lekérdezések.
    /// </summary>
    /// <inheritdoc cref="FakeSqlProvider" />
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class FakeIdSqlProvider: FakeSqlProvider, IEnumerable
    {
        [NotNull]
        ConcurrentDictionary<string, object[][]> ByIdData { get; } = new ConcurrentDictionary<string, object[][]>();

        /// <inheritdoc />
        protected override object[][] FindFakeData(string id, string sql, IEnumerable<object> args)
        {
            object[] parameters =
                args
                ?.Select((a, i) => (object)new Parameter { Name = $"{{{i}}}", Value = a })
                .ToArray()
                ?? Array.Empty<object>();
            string command = string.Format(null, sql ?? "", parameters);
            RegisterQuery(id, command, from Parameter p in parameters where p != null select p.Value);
            if (! string.IsNullOrWhiteSpace(id)
                && ByIdData.TryGetValue(id, out var data))
            {
                return data;
            }
            return Array.Empty<object[]>();
        }

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="id"/> vagy a <paramref name="data"/> értéke <c>null</c>.
        /// </exception>
        public void Add([NotNull] string id, [NotNull] params object[][] data)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ByIdData.TryAdd(id, data);
        }

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="data"/> értéke <c>null</c>.
        /// </exception>
        public void Add(Guid id, [NotNull] params object[][] data) =>
            ByIdData.TryAdd($"{id:N}", data ?? throw new ArgumentNullException(nameof(data)));

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="member">
        ///     Az <c>SQL</c> lekérdezés azonosítójának hívó metódus tagja.
        /// </param>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés függvényen belüli azonosítója.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="data"/> vagy a <paramref name="member"/> értéke <c>null</c>.
        /// </exception>
        public void Add([NotNull] string member, int id, [NotNull] params object[][] data)
        {
            if (string.IsNullOrWhiteSpace(member)) throw new ArgumentNullException(nameof(member));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ByIdData.TryAdd($"{member}/{id}", data);
        }

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="id"/> értéke <c>null</c>.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[][]? this[string id]
        {
            get => ByIdData.TryGetValue(id ?? throw new ArgumentNullException(nameof(id)), out object[][] value)
                        ? value
                        : null;
            set => ByIdData.TryAdd(id ?? throw new ArgumentNullException(nameof(id)), value);
        }

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        [SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[][] this[Guid id]
        {
            get => ByIdData.TryGetValue($"{id:N}", out object[][] value)
                        ? value
                        : null;
            set => ByIdData.TryAdd($"{id:N}", value);
        }

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="member">
        ///     Az <c>SQL</c> lekérdezés azonosítójának hívó metódus tagja.
        /// </param>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés függvényen belüli azonosítója.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     A <paramref name="member"/> értéke <c>null</c>.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[][] this[string member, int id]
        {
            get => ByIdData.TryGetValue($"{member ?? throw new ArgumentNullException(nameof(member))}/{id}", out object[][] value) ? value : null;
            set => ByIdData.TryAdd($"{member ?? throw new ArgumentNullException(nameof(member))}/{id}", value);
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator() => ByIdData.GetEnumerator();

        /// <summary>
        ///     A hamisított lekérdezési eredmények törlése.
        /// </summary>
        public void Clear() => ByIdData.Clear();
    }
}
