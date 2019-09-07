using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

#if NET461

using Microsoft.ServiceBus.Messaging;

#endif

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus
{
#if NET461
    /// <summary>
    /// This represents the extension entity for <see cref="BrokeredMessage"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static class BrokeredMessageExtensions
    {
        /// <summary>
        /// Validates the message against the schema from the given path.
        /// </summary>
        /// <param name="message"><see cref="BrokeredMessage"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="schemaPathPropertyKey">Property key for the schema path.</param>
        /// <returns>Returns the <see cref="BrokeredMessage"/> instance, if validated; otherwise throws an exception.</returns>
        public static async Task<BrokeredMessage> ValidateAsync(this BrokeredMessage message, ISchemaValidator validator, string schemaPathPropertyKey = "schemaPath")
        {
            message.ThrowIfNullOrDefault();
            validator.ThrowIfNullOrDefault();
            schemaPathPropertyKey.ThrowIfNullOrWhiteSpace();

            var payload = default(string);
            using (var stream = message.GetBody<Stream>())
            using (var reader = new StreamReader(stream))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            if (payload.IsNullOrWhiteSpace())
            {
                throw new MessageBodyZeroLengthException().WithServiceBusMessage(message);
            }

            var path = message.Properties[schemaPathPropertyKey] as string;
            if (path.IsNullOrWhiteSpace())
            {
                throw new SchemaPathNotExistException().WithServiceBusMessage(message);
            }

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return message;
        }
    }

#endif
}
