using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using NJsonSchema.Validation;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This represents the exception entity thrown when schema validation fails.
    /// </summary>
    [Serializable]
    public class SchemaValidationException : Exception
    {
        private const string InvalidDataAgainstSchema = "Invalid data against schema";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        public SchemaValidationException()
            : this(InvalidDataAgainstSchema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SchemaValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public SchemaValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected SchemaValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="ISchemaConsumer"/> instance.
        /// </summary>
        public virtual ISchemaConsumer Consumer { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="ValidationError"/> instances.
        /// </summary>
        public List<ValidationError> ValidationErrors { get; private set; }

        /// <summary>
        /// Adds the <see cref="ISchemaConsumer"/> to the exception.
        /// </summary>
        /// <param name="consumer"><see cref="ISchemaConsumer"/> instance.</param>
        /// <returns>Returns the <see cref="SchemaValidationException"/>.</returns>
        public virtual SchemaValidationException WithSchemaConsumer(ISchemaConsumer consumer)
        {
            this.Consumer = consumer.ThrowIfNullOrDefault();

            return this;
        }

        /// <summary>
        /// Adds the list of <see cref="ValidationError"/> instances.
        /// </summary>
        /// <param name="errors">List of <see cref="ValidationError"/> instances.</param>
        /// <returns>Returns the <see cref="SchemaValidationException"/>.</returns>
        public SchemaValidationException WithValidationErrors(List<ValidationError> errors)
        {
            this.ValidationErrors = errors.ThrowIfNullOrDefault();

            return this;
        }
    }
}
