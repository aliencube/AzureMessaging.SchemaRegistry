using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
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
        public virtual ISchemaValidatorPlugin WithValidator(ISchemaValidator validator)
        {
            this.Validator = validator.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public virtual ISchemaValidatorPlugin WithSchemaPathUserPropertyKey(string schemaPathUserPropertyKey)
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
            var body = message.Body;
            if (!body.Any())
            {
                throw new MessageBodyZeroLengthException().WithServiceBusMessage(message);
            }

            var payload = Encoding.UTF8.GetString(message.Body);
            var path = message.UserProperties[this.SchemaPathUserPropertyKey] as string;
            if (path.IsNullOrWhiteSpace())
            {
                throw new SchemaPathNotExistException().WithServiceBusMessage(message);
            }

            var validated = await this.Validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return message;
        }
    }
#endif
}
