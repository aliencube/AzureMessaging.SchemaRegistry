using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

#if NET461

using Microsoft.ServiceBus.Messaging;

#elif NETSTANDARD2_0
using Microsoft.Azure.ServiceBus;
#endif

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus
{
    /// <summary>
    /// This represents the exception entity thrown when message body has zero length.
    /// </summary>
    [Serializable]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class MessageBodyZeroLengthException : Exception
    {
        private const string MessageBodyZeroLength = "Message body has zero length";

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBodyZeroLengthException"/> class.
        /// </summary>
        public MessageBodyZeroLengthException()
            : this(MessageBodyZeroLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBodyZeroLengthException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MessageBodyZeroLengthException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBodyZeroLengthException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception instance.</param>
        public MessageBodyZeroLengthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBodyZeroLengthException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> instance.</param>
        /// <param name="context"><see cref="StreamingContext"/> instance.</param>
        protected MessageBodyZeroLengthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#if NET461

        /// <summary>
        /// Gets the <see cref="BrokeredMessage"/> instance.
        /// </summary>
        public BrokeredMessage ServiceBusMessage { get; private set; }

#elif NETSTANDARD2_0
        /// <summary>
        /// Gets the <see cref="Message"/> instance.
        /// </summary>
        public Message ServiceBusMessage { get; private set; }
#endif

#if NET461

        /// <summary>
        /// Adds the <see cref="BrokeredMessage"/> instance.
        /// </summary>
        /// <param name="message"><see cref="BrokeredMessage"/> instance.</param>
        /// <returns>Returns the <see cref="MessageBodyZeroLengthException"/>.</returns>
        public MessageBodyZeroLengthException WithServiceBusMessage(BrokeredMessage message)
        {
            this.ServiceBusMessage = message.ThrowIfNullOrDefault();

            return this;
        }

#elif NETSTANDARD2_0
        /// <summary>
        /// Adds the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="message"><see cref="Message"/> instance.</param>
        /// <returns>Returns the <see cref="MessageBodyZeroLengthException"/>.</returns>
        public MessageBodyZeroLengthException WithServiceBusMessage(Message message)
        {
            this.ServiceBusMessage = message.ThrowIfNullOrDefault();

            return this;
        }
#endif
    }
}
