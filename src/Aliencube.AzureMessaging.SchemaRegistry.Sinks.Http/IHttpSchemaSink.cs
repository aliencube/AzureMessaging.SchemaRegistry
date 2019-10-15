using System;
using System.Net.Http;
using System.Text;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http
{
    /// <summary>
    /// This provides interfaces to <see cref="HttpSchemaSink"/>.
    /// </summary>
    public interface IHttpSchemaSink : ISchemaSink
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> value.
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Adds base sink location.
        /// </summary>
        /// <param name="location">Base sink location.</param>
        /// <returns>Returns <see cref="ISchemaSink"/> instance.</returns>
        ISchemaSink WithBaseLocation(Uri location);

        /// <summary>
        /// Adds the <see cref="HttpClient"/> instance to the sink.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        ISchemaSink WithHttpClient(HttpClient httpClient);
    }
}
