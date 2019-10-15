using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient
{
    /// <summary>
    /// This represents the extension entity for <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// Sends the <see cref="HttpRequestMessage"/> instance.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="request"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> SendAsync(this System.Net.Http.HttpClient httpClient, HttpRequestMessage request, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            request.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            if (!request.Method.IsSupported())
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }

            if (request.Method != HttpMethod.Get)
            {
                await ValidateAsync(request, validator, path).ConfigureAwait(false);
            }

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (request.Method == HttpMethod.Get)
            {
                await ValidateAsync(response, validator, path).ConfigureAwait(false);

                return response;
            }

            return response;
        }

        /// <summary>
        /// Sends the <see cref="HttpRequestMessage"/> instance.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="request"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> SendAsync(this System.Net.Http.HttpClient httpClient, HttpRequestMessage request, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            request.ThrowIfNullOrDefault();
            cancellationToken.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            if (!request.Method.IsSupported())
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }

            if (request.Method != HttpMethod.Get)
            {
                await ValidateAsync(request, validator, path).ConfigureAwait(false);
            }

            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (request.Method == HttpMethod.Get)
            {
                await ValidateAsync(response, validator, path).ConfigureAwait(false);

                return response;
            }

            return response;
        }

        /// <summary>
        /// Sends the <see cref="HttpRequestMessage"/> instance.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="request"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> value.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> SendAsync(this System.Net.Http.HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption, ISchemaValidator validator, string path)
        {
            httpClient.ThrowIfNullOrDefault();
            request.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            if (!request.Method.IsSupported())
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }

            if (request.Method != HttpMethod.Get)
            {
                await ValidateAsync(request, validator, path).ConfigureAwait(false);
            }

            var response = await httpClient.SendAsync(request, completionOption).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (request.Method == HttpMethod.Get)
            {
                await ValidateAsync(response, validator, path).ConfigureAwait(false);

                return response;
            }

            return response;
        }

        /// <summary>
        /// Sends the <see cref="HttpRequestMessage"/> instance.
        /// </summary>
        /// <param name="httpClient"><see cref="System.Net.Http.HttpClient"/> instance.</param>
        /// <param name="request"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/> value.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        public static async Task<HttpResponseMessage> SendAsync(this System.Net.Http.HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption, ISchemaValidator validator, string path, CancellationToken cancellationToken)
        {
            httpClient.ThrowIfNullOrDefault();
            request.ThrowIfNullOrDefault();
            cancellationToken.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            if (!request.Method.IsSupported())
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }

            if (request.Method != HttpMethod.Get)
            {
                await ValidateAsync(request, validator, path).ConfigureAwait(false);
            }

            var response = await httpClient.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (request.Method == HttpMethod.Get)
            {
                await ValidateAsync(response, validator, path).ConfigureAwait(false);

                return response;
            }

            return response;
        }

        private static async Task<bool> ValidateAsync(HttpRequestMessage request, ISchemaValidator validator, string path)
        {
            var payload = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return validated;
        }
    }
}
