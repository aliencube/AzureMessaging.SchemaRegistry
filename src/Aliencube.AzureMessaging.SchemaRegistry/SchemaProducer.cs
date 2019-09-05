using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This represents the entity for schema producer.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    public class SchemaProducer : ISchemaProducer
    {
        private const string SinkNotFound = "Sink not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaProducer"/> class.
        /// </summary>
        public SchemaProducer()
        {
            this.Sinks = new List<ISchemaSink>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaProducer"/> class.
        /// </summary>
        /// <param name="builder">List of the <see cref="ISchemaBuilder"/> instances.</param>
        public SchemaProducer(ISchemaBuilder builder)
        {
            this.Builder = builder.ThrowIfNullOrDefault();
            this.Sinks = new List<ISchemaSink>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaProducer"/> class.
        /// </summary>
        /// <param name="sinks">List of the <see cref="ISchemaSink"/> instances.</param>
        public SchemaProducer(List<ISchemaSink> sinks)
        {
            this.Sinks = sinks.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaProducer"/> class.
        /// </summary>
        /// <param name="builder">List of the <see cref="ISchemaBuilder"/> instances.</param>
        /// <param name="sinks">List of the <see cref="ISchemaSink"/> instances.</param>
        public SchemaProducer(ISchemaBuilder builder, List<ISchemaSink> sinks)
        {
            this.Builder = builder.ThrowIfNullOrDefault();
            this.Sinks = sinks.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public ISchemaBuilder Builder { get; private set; }

        /// <inheritdoc />
        public List<ISchemaSink> Sinks { get; private set; }

        /// <inheritdoc />
        public ISchemaProducer WithBuilder(ISchemaBuilder builder)
        {
            this.Builder = builder.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public ISchemaProducer WithSink(ISchemaSink sink)
        {
            sink.ThrowIfNullOrDefault();

            this.Sinks.Add(sink);

            return this;
        }

        /// <inheritdoc />
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        public async Task<bool> ProduceAsync(string schema, string path)
        {
            schema.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            if (!this.Sinks.Any())
            {
                throw new InvalidOperationException(SinkNotFound);
            }

            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(this.Sinks, async p => await SetSchema(p, schema, path, exceptions).ConfigureAwait(false));

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> ProduceAsync(Type type, string path)
        {
            var schema = this.Builder.Build(type).ToJson();

            return await this.ProduceAsync(schema, path).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> ProduceAsync<T>(string path)
        {
            return await this.ProduceAsync(typeof(T), path).ConfigureAwait(false);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        private static async Task SetSchema(ISchemaSink sink, string schema, string path, ConcurrentQueue<Exception> exceptions)
        {
            try
            {
                await sink.SetSchemaAsync(schema, path).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }
        }
    }
}
