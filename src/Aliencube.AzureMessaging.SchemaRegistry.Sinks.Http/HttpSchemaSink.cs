using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http
{
    /// <summary>
    /// This represents the schema sink entity over HTTP.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    public class HttpSchemaSink : SchemaSink
    {
        private const string MediaType = "application/json";
        private const string HttpClientNotFound = "HttpClient not found";

        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSchemaSink"/> class.
        /// </summary>
        public HttpSchemaSink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the HTTP sink.</param>
        public HttpSchemaSink(string location)
            : base(location)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSchemaSink"/> class.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        public HttpSchemaSink(HttpClient httpClient)
        {
            this._httpClient = httpClient.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the HTTP sink.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        public HttpSchemaSink(string location, HttpClient httpClient)
            : base(location)
        {
            this._httpClient = httpClient.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> value.
        /// </summary>
        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Adds the <see cref="HttpClient"/> instance to the sink.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        public virtual ISchemaSink WithHttpClient(HttpClient httpClient)
        {
            this._httpClient = httpClient.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        public override async Task<string> GetSchemaAsync(string path)
        {
            path.ThrowIfNullOrWhiteSpace();

            if (this._httpClient.IsNullOrDefault())
            {
                throw new InvalidOperationException(HttpClientNotFound);
            }

            var requestUri = new Uri(string.Join("/", this.BaseLocation.TrimEnd('/'), path.TrimStart('/')).TrimStart('/'));
            var result = await this._httpClient.GetStringAsync(requestUri).ConfigureAwait(false);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        public override async Task<bool> SetSchemaAsync(string schema, string path)
        {
            schema.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            if (this._httpClient.IsNullOrDefault())
            {
                throw new InvalidOperationException(HttpClientNotFound);
            }

            var requestUri = new Uri(string.Join("/", this.BaseLocation.TrimEnd('/'), path.TrimStart('/')).TrimStart('/'));
            using (var content = new StringContent(schema, this.Encoding, MediaType))
            using (var response = await this._httpClient.PutAsync(requestUri, content).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                return true;
            }
        }
    }
}
