using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This provides interfaces to <see cref="SchemaProducer"/>.
    /// </summary>
    public interface ISchemaProducer
    {
        /// <summary>
        /// Gets the <see cref="ISchemaBuilder"/> instance.
        /// </summary>
        ISchemaBuilder Builder { get; }

        /// <summary>
        /// Gets the list of the <see cref="ISchemaSink"/> instances.
        /// </summary>
        List<ISchemaSink> Sinks { get; }

        /// <summary>
        /// Adds the <see cref="ISchemaBuilder"/> instance to the producer.
        /// </summary>
        /// <param name="builder"><see cref="ISchemaBuilder"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaProducer"/> instance.</returns>
        ISchemaProducer WithBuilder(ISchemaBuilder builder);

        /// <summary>
        /// Adds the <see cref="ISchemaSink"/> instance to the producer.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaProducer"/> instance.</returns>
        ISchemaProducer WithSink(ISchemaSink sink);

        /// <summary>
        /// Uploads the schema to all the sinks registered.
        /// </summary>
        /// <param name="schema">JSON schema string.</param>
        /// <param name="path">Schema path to upload.</param>
        /// <returns>Returns <c>True</c>, if the schema is uploaded successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> ProduceAsync(string schema, string path);

        /// <summary>
        /// Uploads the schema to all the sinks registered.
        /// </summary>
        /// <param name="type">Type to upload as JSON schema.</param>
        /// <param name="path">Schema path to upload.</param>
        /// <returns>Returns <c>True</c>, if the schema is uploaded successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> ProduceAsync(Type type, string path);

        /// <summary>
        /// Uploads the schema to all the sinks registered.
        /// </summary>
        /// <typeparam name="T">Type to upload as JSON schema.</typeparam>
        /// <param name="path">Schema path to upload.</param>
        /// <returns>Returns <c>True</c>, if the schema is uploaded successfully; otherwise returns <c>False</c>.</returns>
        Task<bool> ProduceAsync<T>(string path);
    }
}
