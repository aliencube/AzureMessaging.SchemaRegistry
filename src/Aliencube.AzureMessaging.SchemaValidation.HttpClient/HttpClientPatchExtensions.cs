using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient
{
    /// <summary>
    /// This represents the extension entity for <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    [SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
    public static partial class HttpClientExtensions
    {
        private static HttpMethod patch = new HttpMethod("PATCH");

        /// <summary>
        /// Sends the PATCH request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content"><see cref="HttpContent"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this System.Net.Http.HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            content.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            await ValidateAsync(content, validator, path).ConfigureAwait(false);

            using (var request = new HttpRequestMessage(patch, requestUri) { Content = content })
            {
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }

        /// <summary>
        /// Sends the PATCH request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content"><see cref="HttpContent"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this System.Net.Http.HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            content.ThrowIfNullOrDefault();
            cancellationToken.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            await ValidateAsync(content, validator, path).ConfigureAwait(false);

            using (var request = new HttpRequestMessage(patch, requestUri) { Content = content })
            {
                var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }

        /// <summary>
        /// Sends the PATCH request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content"><see cref="HttpContent"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            content.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            await ValidateAsync(content, validator, path).ConfigureAwait(false);

            using (var request = new HttpRequestMessage(patch, requestUri) { Content = content })
            {
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }

        /// <summary>
        /// Sends the PATCH request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="content"><see cref="HttpContent"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            content.ThrowIfNullOrDefault();
            cancellationToken.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            await ValidateAsync(content, validator, path).ConfigureAwait(false);

            using (var request = new HttpRequestMessage(patch, requestUri) { Content = content })
            {
                var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }
    }
}
