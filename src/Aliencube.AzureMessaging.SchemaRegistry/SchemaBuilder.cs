using System;
using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using NJsonSchema;
using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaRegistry
{
    /// <summary>
    /// This represents the entity for JSON schema builder.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    public class SchemaBuilder : ISchemaBuilder
    {
        private JsonSchemaGeneratorSettings _settings;
        private JsonSchemaGenerator _generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaBuilder"/> class.
        /// </summary>
        public SchemaBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaBuilder"/> class.
        /// </summary>
        /// <param name="settings"><see cref="JsonSchemaGeneratorSettings"/> instance.</param>
        public SchemaBuilder(JsonSchemaGeneratorSettings settings)
        {
            this.Settings = settings.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public JsonSchemaGeneratorSettings Settings
        {
            get => this._settings;

            private set
            {
                this._settings = value;
                this._generator = GetJsonSchemaGenerator(value);
            }
        }

        /// <inheritdoc />
        public ISchemaBuilder WithSettings(JsonSchemaGeneratorSettings settings)
        {
            this.Settings = settings.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public JsonSchema Build(Type type)
        {
            type.ThrowIfNullOrDefault();

            var schema = this._generator.Generate(type);

            return schema;
        }

        /// <inheritdoc />
        public JsonSchema Build<T>()
        {
            var schema = this.Build(typeof(T));

            return schema;
        }

        private static JsonSchemaGenerator GetJsonSchemaGenerator(JsonSchemaGeneratorSettings settings)
        {
            var generator = new JsonSchemaGenerator(settings);

            return generator;
        }
    }
}
