using System;
using System.IO;
using System.Threading.Tasks;
#if NET461
using Microsoft.ServiceBus.Messaging;
#elif NETSTANDARD2_0
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
#endif

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus
{
    public class Class1
    {
        public async Task DoSomething()
        {
#if NET461
            var topic = TopicClient.CreateFromConnectionString(string.Empty, string.Empty);
            await topic.SendAsync(new BrokeredMessage()).ConfigureAwait(false);

            var sub = SubscriptionClient.CreateFromConnectionString(string.Empty, string.Empty, string.Empty, ReceiveMode.PeekLock);
            var msg = await sub.PeekAsync().ConfigureAwait(false);

#elif NETSTANDARD2_0
            var topic = new TopicClient(string.Empty, string.Empty);
            await topic.SendAsync(new Message()).ConfigureAwait(false);

            var sub = new SubscriptionClient(string.Empty, string.Empty, string.Empty, ReceiveMode.PeekLock);
            sub.RegisterMessageHandler(async (message, token) =>
            {
                using (var stream = new MemoryStream(message.Body))
                using (var reader = new StreamReader(stream))
                {
                    var payload = await reader.ReadToEndAsync().ConfigureAwait(false);
                }

            }, default(MessageHandlerOptions));

            var sender = new MessageSender(string.Empty, string.Empty);
            await sender.SendAsync(new Message()).ConfigureAwait(false);

            var receiver = new MessageReceiver(string.Empty, string.Empty, ReceiveMode.PeekLock);
            var received = await receiver.PeekAsync().ConfigureAwait(false);
#endif
        }
    }
}
