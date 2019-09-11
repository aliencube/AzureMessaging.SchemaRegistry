using System;

using NJsonSchema;
using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This provides interfaces to <see cref="SchemaBuilder"/>.
    /// </summary>
    public interface ISchemaBuilder
    {
        /// <summary>
        /// Gets the <see cref="JsonSchemaGeneratorSettings"/> instance.
        /// </summary>
        JsonSchemaGeneratorSettings Settings { get; }

        /// <summary>
        /// Adds <see cref="JsonSchemaGeneratorSettings"/>.
        /// </summary>
        /// <param name="settings"><see cref="JsonSchemaGeneratorSettings"/> instance.</param>
        /// <returns></returns>
        ISchemaBuilder WithSettings(JsonSchemaGeneratorSettings settings);

        /// <summary>
        /// Builds JSON schema from the given type.
        /// </summary>
        /// <param name="type">Type to generate JSON schema.</param>
        /// <returns>Returns <see cref="JsonSchema"/> instance.</returns>
        JsonSchema Build(Type type);

        /// <summary>
        /// Builds JSON schema from the given type.
        /// </summary>
        /// <typeparam name="T">Type to generate JSON schema.</typeparam>
        /// <returns>Returns <see cref="JsonSchema"/> instance.</returns>
        JsonSchema Build<T>();
    }
}
