using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This represents the entity for schema consumer.
    /// </summary>
    public interface ISchemaConsumer
    {
        /// <summary>
        /// Gets the <see cref="ISchemaSink"/> instance.
        /// </summary>
        ISchemaSink Sink { get; }

        /// <summary>
        /// Adds the <see cref="ISchemaSink"/> instance to the consumer.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaConsumer"/> instance.</returns>
        ISchemaConsumer WithSink(ISchemaSink sink);

        /// <summary>
        /// Downloads the schema from the sink registered.
        /// </summary>
        /// <param name="path">Schema path to download.</param>
        /// <returns>Returns the JSON schema.</returns>
        Task<string> ConsumeAsync(string path);
    }
}
