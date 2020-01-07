using System.Runtime.Serialization;

namespace Sda.Query
{
    /// <summary>
    ///     Lek�rdez�s param�ter soros�t�shoz.
    /// </summary>
    [DataContract(Name = "Parameter", Namespace = "")]
    public sealed class SqlQueryParameter
    {
        /// <summary>
        ///     T�pus.
        /// </summary>
        [DataMember(Order = 0, EmitDefaultValue = false)]
        public string TypeName { get; set; }

        /// <summary>
        ///     �rt�k.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string Value { get; set; }
    }
}
