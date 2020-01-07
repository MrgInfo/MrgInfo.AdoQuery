using System.Runtime.Serialization;

namespace Sda.Query
{
    /// <summary>
    ///     Lekérdezés paraméter sorosításhoz.
    /// </summary>
    [DataContract(Name = "Parameter", Namespace = "")]
    public sealed class SqlQueryParameter
    {
        /// <summary>
        ///     Típus.
        /// </summary>
        [DataMember(Order = 0, EmitDefaultValue = false)]
        public string TypeName { get; set; }

        /// <summary>
        ///     Érték.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string Value { get; set; }
    }
}
