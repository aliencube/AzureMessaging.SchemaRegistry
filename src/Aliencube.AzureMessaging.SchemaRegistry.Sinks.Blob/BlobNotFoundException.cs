using System;
using System.Runtime.Serialization;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob
{
    /// <summary>
    /// This represents the exception entity thrown when blob container does not exist.
    /// </summary>
    [Serializable]
    public class BlobNotFoundException : Exception
    {
        private const string BlobNotFound = "Blob not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobNotFoundException"/> class.
        /// </summary>
        public BlobNotFoundException()
            : this(BlobNotFound)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public BlobNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public BlobNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected BlobNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
