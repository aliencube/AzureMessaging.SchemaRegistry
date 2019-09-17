using System;
using System.Runtime.Serialization;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob
{
    /// <summary>
    /// This represents the exception entity thrown when blob container does not exist.
    /// </summary>
    [Serializable]
    public class BlobContainerNotFoundException : Exception
    {
        private const string BlobContainerNotFound = "Blob container not found";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobContainerNotFoundException"/> class.
        /// </summary>
        public BlobContainerNotFoundException()
            : this(BlobContainerNotFound)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobContainerNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public BlobContainerNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobContainerNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public BlobContainerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobContainerNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected BlobContainerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
