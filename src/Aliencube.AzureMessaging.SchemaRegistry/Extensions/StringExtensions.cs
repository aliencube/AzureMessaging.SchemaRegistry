using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.HttpClient")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests")]

namespace Aliencube.AzureMessaging.SchemaRegistry.Extensions
{
    /// <summary>
    /// This represents the extension entity for string.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Checks whether the value is null, white-space or not.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Returns <c>True</c>, if the value is null or white-space; otherwise returns <c>False</c>.</returns>
        [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
        internal static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Throws if the given value is null or white-space.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Returns the value.</returns>
        internal static string ThrowIfNullOrWhiteSpace(this string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value;
        }

        /// <summary>
        /// Checks whether the given string value is equivalent to the compared value, considering case-sensitivity.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="comparer">Value to compare.</param>
        /// <param name="comparisonType"><see cref="StringComparison"/> value. Default is <see cref="StringComparison.CurrentCultureIgnoreCase"/>.</param>
        /// <returns>Returns <c>True</c>, if both values are the same as each other; otherwise returns <c>False</c>.</returns>
        internal static bool IsEquivalentTo(this string value, string comparer, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            value.ThrowIfNullOrWhiteSpace();

            return string.Equals(value, comparer, comparisonType);
        }

        /// <summary>
        /// Checks whether the given string starts with the comparer or not, considering case-sensitivity..
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="comparer">Value to check.</param>
        /// <param name="comparisonType"><see cref="StringComparison"/> value. Default is <see cref="StringComparison.CurrentCultureIgnoreCase"/>.</param>
        /// <returns>Returns <c>True</c>, if the given string starts with the comparer; otherwise returns <c>False</c>.</returns>
        internal static bool StartsWithEquivalentOf(this string value, string comparer, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            value.ThrowIfNullOrWhiteSpace();

            if (comparer.IsNullOrWhiteSpace())
            {
                return false;
            }

            return value.StartsWith(comparer, comparisonType);
        }

        /// <summary>
        /// Checks whether the given string ends with the comparer or not, considering case-sensitivity..
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="comparer">Value to check.</param>
        /// <param name="comparisonType"><see cref="StringComparison"/> value. Default is <see cref="StringComparison.CurrentCultureIgnoreCase"/>.</param>
        /// <returns>Returns <c>True</c>, if the given string ends with the comparer; otherwise returns <c>False</c>.</returns>
        internal static bool EndsWithEquivalentOf(this string value, string comparer, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            value.ThrowIfNullOrWhiteSpace();

            if (comparer.IsNullOrWhiteSpace())
            {
                return false;
            }

            return value.EndsWith(comparer, comparisonType);
        }
    }
}
