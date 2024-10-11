using System.Text.RegularExpressions;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Values returned from this function are intentionally lowercased.")]
    public static class StringExtensions
    {
        /// <summary>
        /// Captures instances of uppercase-to-lowercase and lowercase-to-uppercase transitions
        /// that occur _after_ the start of a string and are not already preceded by a hyphen/dash.
        /// </summary>
        private static readonly Regex _casingTransitionPattern = new Regex("(?<!^|-)([A-Z][a-z]|(?<=[a-z])[A-Z])");

        /// <summary>
        /// Converts the supplied <paramref name="value"/> to kebab casing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Uses text case transitions (upper-to-lower/lower-to-upper) in order
        /// to identify opportunitites to inject hypens/dashes.
        /// </para>
        /// <para>
        /// Kebab casing is all lower case with dashes separating words.
        /// </para>
        /// <para>
        /// Example results...
        /// - GetLatestCustomer becomes get-latest-customer
        /// - getLatestCustomer becomes get-latest-customer
        /// </para>
        /// </remarks>
        /// <param name="value">The value to kebab-case.</param>
        /// <returns>Kebab-cased string.</returns>
        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return _casingTransitionPattern.Replace(value, "-$1")
                .Trim()
                .ToLowerInvariant();
        }

        /// <summary>
        /// Converts the supplied <paramref name="value"/> to camel casing.
        /// </summary>
        /// <param name="value">The value to camel-case.</param>
        /// <returns>Camel-cased string</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
        }
    }
}
