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
    public class HttpSchemaSink : SchemaSink, IHttpSchemaSink
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
        /// <param name="location">Base URL of the HTTP sink.</param>
        public HttpSchemaSink(Uri location)
            : this(location.ThrowIfNullOrDefault().ToString())
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
        /// Initializes a new instance of the <see cref="HttpSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the HTTP sink.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        public HttpSchemaSink(Uri location, HttpClient httpClient)
            : this(location.ThrowIfNullOrDefault().ToString())
        {
            this._httpClient = httpClient.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <inheritdoc />
        public virtual ISchemaSink WithBaseLocation(Uri location)
        {
            this.BaseLocation = location.ThrowIfNullOrDefault().ToString();

            return this;
        }

        /// <inheritdoc />
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

            var sanitised = this.SanitisePath(path);
            var requestUri = new Uri(string.Join("/", this.BaseLocation.TrimEnd('/'), sanitised.TrimStart('/')).TrimStart('/'));
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

            var sanitised = this.SanitisePath(path);
            var requestUri = new Uri(string.Join("/", this.BaseLocation.TrimEnd('/'), sanitised.TrimStart('/')).TrimStart('/'));
            using (var content = new StringContent(schema, this.Encoding, MediaType))
            using (var response = await this._httpClient.PutAsync(requestUri, content).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                return true;
            }
        }

        private string SanitisePath(string path)
        {
            if (!path.StartsWithEquivalentOf("http"))
            {
                return path;
            }

            var sanitised = this.BaseLocation.IsNullOrWhiteSpace()
                            ? path.Trim('/')
                            : path.Replace($"{this.BaseLocation.TrimEnd('/')}", string.Empty)
                                  .Trim('/');

            return sanitised;
        }
    }
}
