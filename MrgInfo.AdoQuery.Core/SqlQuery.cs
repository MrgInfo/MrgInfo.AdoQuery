using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using static System.Globalization.CultureInfo;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Egy <c>SQL</c> lek�rdez�s futtat�si k�r�lm�nyeit le�r� oszt�ly.
    /// </summary>
    [DataContract(Name = "Query", Namespace = "")]
    public sealed class SqlQuery
    {
        /// <summary>
        ///     Lek�rdez�s egyedi azonos�t�ja.
        /// </summary>
        [DataMember(Order = 0)]
        public string? Id { get; set; }

        /// <summary>
        ///     Lek�rdez�st futtat� met�dus.
        /// </summary>
        [DataMember(Order = 1)]
        public string? Caller { get; set; }

        /// <summary>
        ///     Az <c>SQL</c> lek�rdez�s.
        /// </summary>
        [DataMember(Order = 2)]
        public string? Command { get; set; }

        /// <summary>
        ///     Lek�rdez�s param�terei.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<object?> Parameters { get; private set; } = new List<object?>();

        /// <summary>
        ///     Lek�rdez�si param�terek soros�t�sa.
        /// </summary>
        /// <exception cref="FormatException">
        ///     Hib�s form�tum.
        /// </exception>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [DataMember(Order = 3, Name =nameof(Parameters))]
        public SqlQueryParameter[] QueryParameters
        {
            get
            {
                var result = new SqlQueryParameter[Parameters.Count];
                var i = 0;
                foreach (object? par in Parameters)
                {
                    result[i++] = par == null
                        ? new SqlQueryParameter()
                        : new SqlQueryParameter
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
        ///     Az deleg�lt seg�ts�g�vel hat�rozhatjuk meg, hogyan cser�lj�k a fikt�v
        ///     adatb�zis azonos�t�kat val�s azonos�t�kra.
        /// </summary>
        public Func<int, int>? IdMapper { get; set; }

        object?[] MapIds(IReadOnlyList<object?> parameters)
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
        ///     Lek�rdez�s futtat�sa.
        /// </summary>
        /// <param name="provider">
        ///     A lek�rdez�st v�grehajt� szolg�ltat�s.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="provider"/> �rt�ke <c>null</c>.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Fut�s sor�n keletkezett hiba.
        /// </exception>
        public void Run(SqlProvider provider)
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
        ///     Lek�rdez�s futtat�sa �s rekordok megsz�ml�l�sa.
        /// </summary>
        /// <param name="provider">
        ///     A lek�rdez�st v�grehajt� szolg�ltat�s.
        /// </param>
        /// <returns>
        ///     Rekordok sz�ma.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="provider"/> �rt�ke <c>null</c>.
        /// </exception>
        /// <exception cref="QueryDbException">
        ///     Fut�s sor�n keletkezett hiba.
        /// </exception>
        public int? RunCount(SqlProvider provider)
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
    }
}
