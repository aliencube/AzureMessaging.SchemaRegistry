using System;
using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: FunctionsStartup(typeof(Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2.StartUp))]
namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    public class StartUp : FunctionsStartup
    {
        private const string StorageConnectionStringKey = "AzureWebJobsStorage";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureJsonSerialiser(builder.Services);
            this.ConfigureSchemaValidation(builder.Services);
        }

        private void ConfigureJsonSerialiser(IServiceCollection services)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            services.AddSingleton<JsonSerializerSettings>(settings);
        }

        private void ConfigureSchemaValidation(IServiceCollection services)
        {
            var blobConnectionString = Environment.GetEnvironmentVariable(StorageConnectionStringKey);
            var blobClient = CloudStorageAccount.Parse(blobConnectionString)
                                                .CreateCloudBlobClient();
            var sink = new BlobStorageSchemaSink(blobClient);
            services.AddSingleton<ISchemaSink, BlobStorageSchemaSink>(_ => sink);
            services.AddSingleton<ISchemaConsumer, SchemaConsumer>();
            services.AddSingleton<ISchemaValidator, SchemaValidator>();
        }
    }
}
