using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob
{
    /// <summary>
    /// This represents the schema sink entity for Azure Blob Storage.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class BlobStorageSchemaSink : SchemaSink, IBlobStorageSchemaSink
    {
        private const string DefaultContainerName = "schemas";
        private const string ContentType = "application/json";

        private CloudBlobClient _blobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        public BlobStorageSchemaSink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the Azure Blob Storage instance.</param>
        public BlobStorageSchemaSink(string location)
            : base(location)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the Azure Blob Storage instance.</param>
        public BlobStorageSchemaSink(Uri location)
            : base(location.ThrowIfNullOrDefault().ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        /// <param name="blobClient"><see cref="CloudBlobClient"/> instance.</param>
        public BlobStorageSchemaSink(CloudBlobClient blobClient)
        {
            this._blobClient = blobClient.ThrowIfNullOrDefault();
            this.BaseLocation = blobClient.BaseUri.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the Azure Blob Storage instance.</param>
        /// <param name="blobClient"><see cref="CloudBlobClient"/> instance.</param>
        public BlobStorageSchemaSink(string location, CloudBlobClient blobClient)
            : base(location)
        {
            this._blobClient = blobClient.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base URL of the Azure Blob Storage instance.</param>
        /// <param name="blobClient"><see cref="CloudBlobClient"/> instance.</param>
        public BlobStorageSchemaSink(Uri location, CloudBlobClient blobClient)
            : base(location.ThrowIfNullOrDefault().ToString())
        {
            this._blobClient = blobClient.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public virtual string Container { get; private set; } = DefaultContainerName;

        /// <inheritdoc />
        public virtual ISchemaSink WithBaseLocation(Uri location)
        {
            this.BaseLocation = location.ThrowIfNullOrDefault().ToString();

            return this;
        }

        /// <inheritdoc />
        public virtual ISchemaSink WithBlobClient(CloudBlobClient blobClient)
        {
            this._blobClient = blobClient.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public virtual ISchemaSink WithContainer(string container)
        {
            this.Container = container.ThrowIfNullOrWhiteSpace();

            return this;
        }

        /// <inheritdoc />
        public override async Task<string> GetSchemaAsync(string path)
        {
            path.ThrowIfNullOrWhiteSpace();

            var sanitised = this.SanitisePath(path);
            var container = await this.GetBlobContainerAsync().ConfigureAwait(false);
            var blob = await this.GetBlobAsync(container, sanitised).ConfigureAwait(false);

            var schema = await blob.DownloadTextAsync().ConfigureAwait(false);

            return schema;
        }

        /// <inheritdoc />
        public override async Task<bool> SetSchemaAsync(string schema, string path)
        {
            schema.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            var sanitised = this.SanitisePath(path);
            var container = await this.GetBlobContainerAsync(createIfNotExists: true).ConfigureAwait(false);
            var blob = await this.GetBlobAsync(container, sanitised, createIfNotExists: true).ConfigureAwait(false);

            await blob.UploadTextAsync(schema).ConfigureAwait(false);

            return true;
        }

        private string SanitisePath(string path)
        {
            if (!path.StartsWithEquivalentOf("http"))
            {
                return path;
            }

            var sanitised = path.Replace($"{this.BaseLocation.TrimEnd('/')}/", string.Empty)
                                .Replace($"{this.Container.Trim('/')}/", string.Empty)
                                .Trim('/');

            return sanitised;
        }

        private async Task<CloudBlobContainer> GetBlobContainerAsync(bool createIfNotExists = false)
        {
            var container = this._blobClient.GetContainerReference(this.Container);

            if (createIfNotExists)
            {
                await container.CreateIfNotExistsAsync().ConfigureAwait(false);

                return container;
            }

            var exists = await container.ExistsAsync().ConfigureAwait(false);
            if (!exists)
            {
                throw new BlobContainerNotFoundException();
            }

            return container;
        }

        private async Task<CloudBlockBlob> GetBlobAsync(CloudBlobContainer container, string path, bool createIfNotExists = false)
        {
            var blob = container.GetBlockBlobReference(path);
            blob.Properties.ContentType = ContentType;

            if (createIfNotExists)
            {
                return blob;
            }

            var exists = await blob.ExistsAsync().ConfigureAwait(false);
            if (!exists)
            {
                throw new BlobNotFoundException();
            }

            return blob;
        }
    }
}
