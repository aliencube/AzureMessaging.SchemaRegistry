using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Style", "IDE0021:Use expression body for constructors")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    public class ServiceBusTopicPublishHttpTrigger
    {
        private readonly JsonSerializerSettings _settings;
        private readonly ISchemaValidator _validator;

        public ServiceBusTopicPublishHttpTrigger(JsonSerializerSettings settings, ISchemaValidator validator)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [FunctionName(nameof(ServiceBusTopicPublishHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "servicebus/publish")] HttpRequest req,
            [ServiceBus("%ServiceBusTopic%", EntityType.Topic, Connection = "AzureServiceBusConnectionString")] IAsyncCollector<string> collector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var path = "default.json";

            var sample = new SampleClass();
            var payload = await JsonConvert.SerializeObject(sample, this._settings)
                                           .ValidateAsStringAsync(this._validator, path).ConfigureAwait(false);

            await collector.AddAsync(payload).ConfigureAwait(false);

            return new StatusCodeResult((int)HttpStatusCode.Created);
        }
    }
}
