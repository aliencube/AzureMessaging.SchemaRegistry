using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

using NJsonSchema.Validation;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This represents the exception entity thrown when schema validation fails.
    /// </summary>
    [Serializable]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
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
        /// Gets the <see cref="ISchemaSink"/> instance.
        /// </summary>
        public ISchemaSink Sink { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="ValidationError"/> instances.
        /// </summary>
        public List<ValidationError> ValidationErrors { get; private set; }

        /// <summary>
        /// Adds the <see cref="ISchemaSink"/> instance.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <returns>Returns the <see cref="SchemaValidationException"/>.</returns>
        public SchemaValidationException WithSink(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();

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
