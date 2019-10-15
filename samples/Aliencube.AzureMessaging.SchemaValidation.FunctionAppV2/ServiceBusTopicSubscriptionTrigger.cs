using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusTopicSubscriptionTrigger
    {
        private const string SchemaVersionKey = "SchemaVersion";
        private const string SchemaFilenameKey = "SchemaFilename";

        private readonly ISchemaValidator _validator;

        public ServiceBusTopicSubscriptionTrigger(ISchemaValidator validator)
        {
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [FunctionName(nameof(ServiceBusTopicSubscriptionTrigger))]
        public async Task Run(
            [ServiceBusTrigger("%ServiceBusTopic%", "%ServiceBusTopicSubscription%", Connection = "AzureServiceBusConnectionString")] string message,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {message}");

            var version = Environment.GetEnvironmentVariable(SchemaVersionKey);
            var filename = Environment.GetEnvironmentVariable(SchemaFilenameKey);
            var path = $"{version}/{filename}";

            var validated = await message.ValidateAsync(this._validator, path).ConfigureAwait(false);

            log.LogInformation($"Message validated against schema.");
        }
    }
}
