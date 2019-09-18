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

            var cloned = message.Clone();

            var payload = default(string);
            using (var stream = cloned.GetBody<Stream>())
            using (var reader = new StreamReader(stream))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            if (payload.IsNullOrWhiteSpace())
            {
                throw new MessageBodyZeroLengthException().WithServiceBusMessage(cloned);
            }

            var path = cloned.Properties[schemaPathPropertyKey] as string;
            if (path.IsNullOrWhiteSpace())
            {
                throw new SchemaPathNotExistException().WithServiceBusMessage(cloned);
            }

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return message;
        }

        /// <summary>
        /// Validates the message against the schema from the given path.
        /// </summary>
        /// <param name="message"><see cref="BrokeredMessage"/> instance.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="schemaPathPropertyKey">Property key for the schema path.</param>
        /// <returns>Returns the <see cref="BrokeredMessage"/> instance, if validated; otherwise throws an exception.</returns>
        public static async Task<BrokeredMessage> ValidateAsync(this Task<BrokeredMessage> message, ISchemaValidator validator, string schemaPathPropertyKey = "schemaPath")
        {
            var instance = await message.ConfigureAwait(false);

            return await instance.ValidateAsync(validator, schemaPathPropertyKey).ConfigureAwait(false);
        }
    }

#endif
}
