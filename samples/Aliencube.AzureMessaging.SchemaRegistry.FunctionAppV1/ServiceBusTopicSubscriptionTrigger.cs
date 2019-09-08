using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaValidation;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;

namespace Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV1
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
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
            var validator = new SchemaValidator().WithSink(sink);

            var validated = await message.ValidateAsync(validator, path).ConfigureAwait(false);

            log.LogInformation($"Message validated against schema.");
        }
    }
}
