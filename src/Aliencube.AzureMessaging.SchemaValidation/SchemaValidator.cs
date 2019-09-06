using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using NJsonSchema;
using NJsonSchema.Validation;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This represents the validator entity against JSON schema.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    public class SchemaValidator : ISchemaValidator
    {
        private readonly JsonSchemaValidator _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidator"/> class.
        /// </summary>
        public SchemaValidator()
        {
            this._validator = new JsonSchemaValidator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidator"/> class.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        public SchemaValidator(ISchemaSink sink)
            : this()
        {
            this.Sink = sink.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public virtual ISchemaSink Sink { get; private set; }

        /// <inheritdoc />
        public virtual ISchemaValidator WithSink(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public virtual async Task<bool> ValidateAsync(string payload, string path)
        {
            payload.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            var schema = await this.Sink.GetSchemaAsync(path).ConfigureAwait(false);
            if (schema.IsNullOrWhiteSpace())
            {
                throw new SchemaNotFoundException()
                          .WithSink(this.Sink);
            }

            var jschema = default(JsonSchema);
            try
            {
                jschema = await JsonSchema.FromJsonAsync(schema).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SchemaMalformedException(ex)
                          .WithSink(this.Sink);
            }

            var errors = this._validator.Validate(payload, jschema);
            if (errors.Any())
            {
                throw new SchemaValidationException()
                          .WithSink(this.Sink)
                          .WithValidationErrors(errors.ToList());
            }

            return true;
        }
    }
}
