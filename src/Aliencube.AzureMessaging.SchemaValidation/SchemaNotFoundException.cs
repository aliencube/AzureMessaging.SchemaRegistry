using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This represents the exception entity thrown when schema does not exist.
    /// </summary>
    [Serializable]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class SchemaNotFoundException : Exception
    {
        private const string SchemaNotFound = "Schema not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaNotFoundException"/> class.
        /// </summary>
        public SchemaNotFoundException()
            : this(SchemaNotFound)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SchemaNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public SchemaNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected SchemaNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="ISchemaSink"/> instance.
        /// </summary>
        public ISchemaSink Sink { get; private set; }

        /// <summary>
        /// Adds the <see cref="ISchemaSink"/> instance.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <returns>Returns the <see cref="SchemaNotFoundException"/>.</returns>
        public SchemaNotFoundException WithSink(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();

            return this;
        }
    }
}
