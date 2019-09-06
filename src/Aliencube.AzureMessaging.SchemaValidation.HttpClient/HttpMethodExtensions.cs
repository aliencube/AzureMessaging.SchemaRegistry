using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests")]

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient
{
    /// <summary>
    /// This represents the extension entity for <see cref="HttpMethod"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    internal static class HttpMethodExtensions
    {
        /// <summary>
        /// Checks whether the HTTP method is supported by this library or not.
        /// </summary>
        /// <param name="method"><see cref="HttpMethod"/> instance.</param>
        /// <returns>Returns <c>True</c>, if supported; otherwise returns <c>False</c>.</returns>
        internal static bool IsSupported(this HttpMethod method)
        {
            method.ThrowIfNullOrDefault();

            if (method == HttpMethod.Get)
            {
                return true;
            }

            if (method == HttpMethod.Post)
            {
                return true;
            }

            if (method == HttpMethod.Put)
            {
                return true;
            }

            if (method == new HttpMethod("PATCH"))
            {
                return true;
            }

            return false;
        }
    }
}
