using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: FunctionsStartup(typeof(Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2.StartUp))]

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    public class StartUp : FunctionsStartup
    {
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
            var location = Utility.GetBasePath();
            var sink = new FileSystemSchemaSink(location);
            services.AddSingleton<ISchemaSink, FileSystemSchemaSink>(_ => sink);
            services.AddSingleton<ISchemaConsumer, SchemaConsumer>();
            services.AddSingleton<ISchemaValidator, SchemaValidator>();
        }
    }
}
