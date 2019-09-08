using Microsoft.WindowsAzure.Storage.Blob;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob
{
    /// <summary>
    /// This provides interfaces to <see cref="BlobStorageSchemaSink"/>.
    /// </summary>
    public interface IBlobStorageSchemaSink : ISchemaSink
    {
        /// <summary>
        /// Gets the blob container name.
        /// </summary>
        string Container { get; }

        /// <summary>
        /// Adds the <see cref="CloudBlobClient"/> instance to the sink.
        /// </summary>
        /// <param name="blobClient"><see cref="CloudBlobClient"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        ISchemaSink WithBlobClient(CloudBlobClient blobClient);

        /// <summary>
        /// Adds the blob container name to the sink.
        /// </summary>
        /// <param name="container">Blob container name.</param>
        /// <returns>Returns the <see cref="ISchemaSink"/> instance.</returns>
        ISchemaSink WithContainer(string container);
    }
}
