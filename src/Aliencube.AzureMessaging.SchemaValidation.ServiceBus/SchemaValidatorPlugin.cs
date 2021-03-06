using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

#if NETSTANDARD2_0
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
#endif

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus
{
#if NETSTANDARD2_0
    /// <summary>
    /// This represents the plug-in entity for schema validation in Azure Service Bus.
    /// </summary>
    public class SchemaValidatorPlugin : ServiceBusPlugin, ISchemaValidatorPlugin
    {
        private const string SchemaPathKey = "schemaPath";

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidatorPlugin"/> class.
        /// </summary>
        public SchemaValidatorPlugin()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidatorPlugin"/> class.
        /// </summary>
        /// <param name="shouldContinueOnException">Value indicating whether to continue processing on exception or not.</param>
        public SchemaValidatorPlugin(bool shouldContinueOnException)
        {
            this.ShouldContinueOnException = shouldContinueOnException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidatorPlugin"/> class.
        /// </summary>
        /// <param name="schemaPathUserPropertyKey">User property key in a message to get the schema path.</param>
        /// <param name="shouldContinueOnException">Value indicating whether to continue processing on exception or not.</param>
        public SchemaValidatorPlugin(string schemaPathUserPropertyKey, bool shouldContinueOnException)
        {
            this.SchemaPathUserPropertyKey = schemaPathUserPropertyKey.ThrowIfNullOrWhiteSpace();
            this.ShouldContinueOnException = shouldContinueOnException;
        }

        /// <inheritdoc />
        public override string Name => this.GetType().FullName;

        /// <inheritdoc />
        public override bool ShouldContinueOnException { get; }

        /// <inheritdoc />
        public virtual ISchemaValidator Validator { get; private set; }

        /// <inheritdoc />
        public virtual string SchemaPathUserPropertyKey { get; private set; } = SchemaPathKey;

        /// <inheritdoc />
        public virtual ServiceBusPlugin WithValidator(ISchemaValidator validator)
        {
            this.Validator = validator.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public virtual ServiceBusPlugin WithSchemaPathUserPropertyKey(string schemaPathUserPropertyKey)
        {
            this.SchemaPathUserPropertyKey = schemaPathUserPropertyKey.ThrowIfNullOrWhiteSpace();

            return this;
        }

        /// <inheritdoc />
        public override async Task<Message> BeforeMessageSend(Message message)
        {
            message.ThrowIfNullOrDefault();

            return await this.ValidateAsync(message).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task<Message> AfterMessageReceive(Message message)
        {
            message.ThrowIfNullOrDefault();

            return await this.ValidateAsync(message).ConfigureAwait(false);
        }

        private async Task<Message> ValidateAsync(Message message)
        {
            var cloned = message.Clone();

            var body = cloned.Body;
            if (!body.Any())
            {
                throw new MessageBodyZeroLengthException().WithServiceBusMessage(cloned);
            }

            var payload = Encoding.UTF8.GetString(cloned.Body);
            var path = cloned.UserProperties[this.SchemaPathUserPropertyKey] as string;
            if (path.IsNullOrWhiteSpace())
            {
                throw new SchemaPathNotExistException().WithServiceBusMessage(cloned);
            }

            var validated = await this.Validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return message;
        }
    }
#endif
}
