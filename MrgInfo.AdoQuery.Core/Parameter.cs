using System;
using System.Diagnostics.CodeAnalysis;

namespace MrgInfo.AdoQuery.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Query parameter.
    /// </summary>
    sealed class Parameter: IFormattable
    {
        /// <summary>
        ///     Name of parameter.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Value of parameter.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        ///     Present in the query command?
        /// </summary>
        public bool Present { get; private set; }

        /// <inheritdoc />
        public override string ToString() => $"{Name ?? "N/A"} = {Value ?? "N/A"}";

        /// <inheritdoc />
        /// <exception cref="FormatException">
        ///     Invalid format!
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider")]
        [SuppressMessage("ReSharper", "InvertIf")]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "==":
                    if (string.IsNullOrEmpty(Name)) goto case default;
                    if (Value is null || Value is DBNull)
                    {
                        Present = false;
                        return "is null";
                    }
                    Present = true;
                    return $"= {Name}";
                case "!=":
                    if (string.IsNullOrEmpty(Name)) goto case default;
                    if (Value is null || Value is DBNull)
                    {
                        Present = false;
                        return "is not null";
                    }
                    Present = true;
                    return $"<> {Name}";
                case "=*":
                    if (string.IsNullOrEmpty(Name)) goto case default;
                    Value = $"{Value}%";
                    Present = true;
                    return $"like {Name}";
                case "*=":
                    if (string.IsNullOrEmpty(Name)) goto case default;
                    Value = $"%{Value}";
                    Present = true;
                    return $"like {Name}";
                case null:
                case "":
                    if (string.IsNullOrEmpty(Name)) goto case default;
                    Present = true;
                    return Name;
                default:
                    throw new FormatException();
            }
        }
    }
}
