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
        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, string requestUri, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, string requestUri, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();
            cancellationToken.ThrowIfNullOrDefault();

            var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, string requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var response = await httpClient.GetAsync(requestUri, completionOption).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, string requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();
            cancellationToken.ThrowIfNullOrDefault();

            var response = await httpClient.GetAsync(requestUri, completionOption, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();
            cancellationToken.ThrowIfNullOrDefault();

            var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var response = await httpClient.GetAsync(requestUri, completionOption).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();
            cancellationToken.ThrowIfNullOrDefault();

            var response = await httpClient.GetAsync(requestUri, completionOption, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            await ValidateAsync(response, validator, path).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns the string response.</returns>
        public static async Task<string> GetStringAsync(this System.Net.Http.HttpClient httpClient, string requestUri, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var payload = await httpClient.GetStringAsync(requestUri).ConfigureAwait(false);

            await ValidateAsync(payload, validator, path).ConfigureAwait(false);

            return payload;
        }

        /// <summary>
        /// Sends the GET request to the given URI.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns the string response.</returns>
        public static async Task<string> GetStringAsync(this System.Net.Http.HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            requestUri.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var payload = await httpClient.GetStringAsync(requestUri).ConfigureAwait(false);

            await ValidateAsync(payload, validator, path).ConfigureAwait(false);

            return payload;
        }

        private static async Task<bool> ValidateAsync(HttpResponseMessage response, ISchemaValidator validator, string path)
        {
            var payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return validated;
        }

        private static async Task<bool> ValidateAsync(string payload, ISchemaValidator validator, string path)
        {
            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return validated;
        }
    }
}
