using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MrgInfo.AdoQuery.Core.Fake
{
    /// <inheritdoc cref="FakeSqlProvider" />
    /// <summary>
    ///     Egyedi azonosítók segítségével hamisított lekérdezések.
    /// </summary>
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
    [SuppressMessage("Design", "CA1010:Collections should implement generic interface")]
    public sealed class FakeIdSqlProvider: FakeSqlProvider, IEnumerable
    {
        ConcurrentDictionary<string, IList<IList<object?>>?> ByIdData { get; } = new ConcurrentDictionary<string, IList<IList<object?>>?>();

        /// <inheritdoc />
        protected override IList<IList<object?>>? FindFakeData(string? id, string? query, IEnumerable<object?>? args)
        {
            object[] parameters =
                args
                ?.Select((a, i) => (object)new Parameter { Name = $"{{{i}}}", Value = a })
                .ToArray()
                ?? Array.Empty<object>();
            string command = string.Format(null, query ?? "", parameters);
            RegisterQuery(id, command, from Parameter p in parameters where p != null select p.Value);
            if (! string.IsNullOrWhiteSpace(id)
                && ByIdData.TryGetValue(id, out IList<IList<object?>>? data))
            {
                return data;
            }
            return Array.Empty<IList<object?>>();
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
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public void Add(string id, params object?[][]? data) =>
            ByIdData.TryAdd(id, data);

        /// <summary>
        ///     Egy hamisított lekérdezés eredmény hozzáadása a rendszerhez.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <param name="data">
        ///     A találati adathalmazt reprezentáló objektumok.
        /// </param>
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public void Add(Guid id, params object?[][]? data) =>
            ByIdData.TryAdd($"{id:N}", data);

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
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public void Add(string member, int id, params object?[][]? data) =>
            ByIdData.TryAdd($"{member}/{id}", data);

        /// <summary>
        ///     Hamisított lekérdezések.
        /// </summary>
        /// <param name="id">
        ///     Az <c>SQL</c> lekérdezés egyedi azonosítója.
        /// </param>
        /// <value>
        ///     A lekérdezés eredményének hamisított helyettesítője.
        /// </value>
        public IList<IList<object?>>? this[string id]
        {
            get => ByIdData.TryGetValue(id, out IList<IList<object?>>? value)
                ? value
                : null;
            set => ByIdData.TryAdd(id, value);
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
        [SuppressMessage("Design", "CA1043:Use Integral Or String Argument For Indexers")]
        public IList<IList<object?>>? this[Guid id]
        {
            get => ByIdData.TryGetValue($"{id:N}", out IList<IList<object?>>? value)
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
        public IList<IList<object?>>? this[string member, int id]
        {
            get => ByIdData.TryGetValue($"{member}/{id}", out IList<IList<object?>>? value)
                ? value
                : null;
            set => ByIdData.TryAdd($"{member}/{id}", value);
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator() => ByIdData.GetEnumerator();

        /// <summary>
        ///     A hamisított lekérdezési eredmények törlése.
        /// </summary>
        public void Clear() => ByIdData.Clear();
    }
}
