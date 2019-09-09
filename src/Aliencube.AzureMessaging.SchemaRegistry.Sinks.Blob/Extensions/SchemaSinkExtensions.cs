using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="SchemaSink"/>.
    /// </summary>
    public static class SchemaSinkExtensions
    {
        /// <summary>
        /// Adds the <see cref="CloudBlobClient"/> instance to the sink.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <param name="blobClient"><see cref="CloudBlobClient"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        public static ISchemaSink WithBlobClient(this ISchemaSink sink, CloudBlobClient blobClient)
        {
            sink.ThrowIfNullOrDefault();
            blobClient.ThrowIfNullOrDefault();

            var instance = (sink as BlobStorageSchemaSink).ThrowIfNullOrDefault();

            instance.WithBlobClient(blobClient);

            return instance;
        }

        /// <summary>
        /// Adds the <see cref="CloudBlobClient"/> instance to the sink.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <param name="container">Blob container name.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        public static ISchemaSink WithContainer(this ISchemaSink sink, string container)
        {
            sink.ThrowIfNullOrDefault();
            container.ThrowIfNullOrWhiteSpace();

            var instance = (sink as BlobStorageSchemaSink).ThrowIfNullOrDefault();

            instance.WithContainer(container);

            return instance;
        }
    }
}
