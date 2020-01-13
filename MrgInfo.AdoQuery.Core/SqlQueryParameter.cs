using System.Runtime.Serialization;

namespace MrgInfo.AdoQuery.Core
{
    /// <summary>
    ///     Serializing query parameters.
    /// </summary>
    [DataContract(Name = "Parameter", Namespace = "")]
    public sealed class SqlQueryParameter
    {
        /// <summary>
        ///     Type of parameter.
        /// </summary>
        [DataMember(Order = 0, EmitDefaultValue = false)]
        public string? TypeName { get; set; }

        /// <summary>
        ///     Value of parameter.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string? Value { get; set; }
    }
}
