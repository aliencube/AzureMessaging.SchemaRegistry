using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV1
{
    [ExcludeFromCodeCoverage]
    public static class ServiceBusTopicSubscriptionTrigger
    {
        [FunctionName(nameof(ServiceBusTopicSubscriptionTrigger))]
        public static async Task Run(
            [ServiceBusTrigger("%ServiceBusTopic%", "%ServiceBusTopicSubscription%", AccessRights.Manage, Connection = "AzureServiceBusConnectionString")] string message,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {message}");

            var location = Utility.GetBasePath();
            var path = "default.json";
            var sink = new FileSystemSchemaSink().WithBaseLocation(location);
            var consumer = new SchemaConsumer().WithSink(sink);
            var validator = new SchemaValidator().WithSchemaConsumer(consumer);

            var validated = await message.ValidateAsync(validator, path).ConfigureAwait(false);

            log.LogInformation($"Message validated against schema.");
        }
    }
}
