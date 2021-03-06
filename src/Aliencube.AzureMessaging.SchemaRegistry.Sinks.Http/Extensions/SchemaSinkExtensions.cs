using System.Net.Http;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="SchemaSink"/>.
    /// </summary>
    public static class SchemaSinkExtensions
    {
        /// <summary>
        /// Adds the <see cref="HttpClient"/> instance to the sink.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        public static ISchemaSink WithHttpClient(this ISchemaSink sink, HttpClient httpClient)
        {
            sink.ThrowIfNullOrDefault();
            httpClient.ThrowIfNullOrDefault();

            var instance = (sink as HttpSchemaSink).ThrowIfNullOrDefault();

            instance.WithHttpClient(httpClient);

            return instance;
        }
    }
}
