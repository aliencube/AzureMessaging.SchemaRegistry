using System;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This represents the entity for schema consumer.
    /// </summary>
    public class SchemaConsumer : ISchemaConsumer
    {
        private const string SinkNotFound = "Sink not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaConsumer"/> class.
        /// </summary>
        public SchemaConsumer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaConsumer"/> class.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        public SchemaConsumer(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public ISchemaSink Sink { get; private set; }

        /// <inheritdoc />
        public ISchemaConsumer WithSink(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public async Task<string> ConsumeAsync(string path)
        {
            path.ThrowIfNullOrWhiteSpace();

            if (this.Sink.IsNullOrDefault())
            {
                throw new InvalidOperationException(SinkNotFound);
            }

            var schema = await this.Sink.GetSchemaAsync(path).ConfigureAwait(false);

            return schema;
        }
    }
}
