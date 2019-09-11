using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaValidation;
using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class ServiceBusTopicSubscriptionTrigger
    {
        private readonly ISchemaValidator _validator;

        public ServiceBusTopicSubscriptionTrigger(ISchemaValidator validator)
        {
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [FunctionName(nameof(ServiceBusTopicSubscriptionTrigger))]
        public async Task Run(
            [ServiceBusTrigger("%ServiceBusTopic%", "%ServiceBusTopicSubscription%", Connection = "AzureServiceBusConnectionString")]string message,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {message}");

            var path = "default.json";

            var validated = await message.ValidateAsync(this._validator, path).ConfigureAwait(false);

            log.LogInformation($"Message validated against schema.");
        }
    }
}
