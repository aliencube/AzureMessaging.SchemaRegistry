using System.Threading.Tasks;

#if NETSTANDARD2_0
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
#endif

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus
{
#if NETSTANDARD2_0
    /// <summary>
    /// This provides interfaces to <see cref="SchemaValidatorPlugin"/>.
    /// </summary>
    public interface ISchemaValidatorPlugin
    {
        /// <summary>
        /// Gets the name of the plug-in.
        /// </summary>
        /// <remarks>
        /// This name is used to identify the plug-in, and prevent a plug-in from being registered multiple times.
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Value indicating whether or not, the exception in the plug-in should prevent a send or receive operation.
        /// </summary>
        bool ShouldContinueOnException { get; }

        /// <summary>
        /// Gets the <see cref="ISchemaValidator"/> instance.
        /// </summary>
        ISchemaValidator Validator { get; }

        /// <summary>
        /// Gets the user property key in a message to get the schema path.
        /// </summary>
        string SchemaPathUserPropertyKey { get; }

        /// <summary>
        /// Adds the <see cref="ISchemaValidator"/> instance to the plug-in.
        /// </summary>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <returns>Returns the <see cref="ServiceBusPlugin"/> instance.</returns>
        ServiceBusPlugin WithValidator(ISchemaValidator validator);

        /// <summary>
        /// Adds the user property key in a message to get the schema path.
        /// </summary>
        /// <param name="schemaPathUserPropertyKey">User property key in a message to get the schema path.</param>
        /// <returns>Returns the <see cref="ServiceBusPlugin"/> instance.</returns>
        ServiceBusPlugin WithSchemaPathUserPropertyKey(string schemaPathUserPropertyKey);

        /// <summary>
        /// Performs an operation before the message is sent.
        /// </summary>
        /// <param name="message"><see cref="Message"/> instance.</param>
        /// <returns>Returns the <see cref="Message"/> instance, if validation passes.</returns>
        Task<Message> BeforeMessageSend(Message message);

        /// <summary>
        /// Performs an operation after the message is received, but before it is returned to <see cref="IMessageReceiver"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/> instance.</param>
        /// <returns>Returns the <see cref="Message"/> instance, if validation passes.</returns>
        Task<Message> AfterMessageReceive(Message message);
    }
#endif
}
