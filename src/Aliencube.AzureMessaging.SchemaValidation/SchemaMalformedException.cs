using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This represents the exception entity thrown when schema is not formed properly.
    /// </summary>
    [Serializable]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class SchemaMalformedException : Exception
    {
        private const string SchemaNotFormed = "Schema is not formed properly";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaMalformedException"/> class.
        /// </summary>
        public SchemaMalformedException()
            : this(SchemaNotFormed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaMalformedException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SchemaMalformedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaMalformedException"/> class.
        /// </summary>
        /// <param name="innerException">Inner exception instance.</param>
        public SchemaMalformedException(Exception innerException)
            : this(SchemaNotFormed, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaMalformedException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public SchemaMalformedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaMalformedException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        [ExcludeFromCodeCoverage]
        protected SchemaMalformedException(SerializationInfo info, StreamingContext context)
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
        /// <returns>Returns the <see cref="SchemaMalformedException"/>.</returns>
        public SchemaMalformedException WithSink(ISchemaSink sink)
        {
            this.Sink = sink.ThrowIfNullOrDefault();

            return this;
        }
    }
}
