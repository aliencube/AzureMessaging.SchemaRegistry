using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;
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
        /// <param name="consumer"><see cref="ISchemaConsumer"/> instance.</param>
        public SchemaValidator(ISchemaConsumer consumer)
            : this()
        {
            this.Consumer = consumer.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public virtual ISchemaConsumer Consumer { get; private set; }

        /// <inheritdoc />
        public virtual ISchemaValidator WithSchemaConsumer(ISchemaConsumer consumer)
        {
            this.Consumer = consumer.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public virtual async Task<bool> ValidateAsync(string payload, string path)
        {
            payload.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            var schema = await this.Consumer.ConsumeAsync(path).ConfigureAwait(false);
            if (schema.IsNullOrWhiteSpace())
            {
                throw new SchemaNotFoundException()
                          .WithSchemaConsumer(this.Consumer);
            }

            var jschema = default(JsonSchema);
            try
            {
                jschema = await JsonSchema.FromJsonAsync(schema).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SchemaMalformedException(ex)
                          .WithSchemaConsumer(this.Consumer);
            }

            var errors = this._validator.Validate(payload, jschema);
            if (errors.Any())
            {
                throw new SchemaValidationException()
                          .WithSchemaConsumer(this.Consumer)
                          .WithValidationErrors(errors.ToList());
            }

            return true;
        }
    }
}
