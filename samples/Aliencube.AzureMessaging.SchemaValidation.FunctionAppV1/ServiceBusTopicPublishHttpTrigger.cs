using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV1
{
    [ExcludeFromCodeCoverage]
    public static class ServiceBusTopicPublishHttpTrigger
    {
        [FunctionName(nameof(ServiceBusTopicPublishHttpTrigger))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "servicebus/publish")] HttpRequestMessage req,
            [ServiceBus("%ServiceBusTopic%", Connection = "AzureServiceBusConnectionString", EntityType = EntityType.Topic)] IAsyncCollector<string> collector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            var location = Utility.GetBasePath();
            var path = "default.json";
            var sink = new FileSystemSchemaSink().WithBaseLocation(location);
            var consumer = new SchemaConsumer().WithSink(sink);
            var validator = new SchemaValidator().WithSchemaConsumer(consumer);

            var sample = new SampleClass();
            var payload = await JsonConvert.SerializeObject(sample, settings)
                                           .ValidateAsStringAsync(validator, path).ConfigureAwait(false);

            await collector.AddAsync(payload).ConfigureAwait(false);

            return req.CreateResponse(HttpStatusCode.Created);
        }
    }
}
