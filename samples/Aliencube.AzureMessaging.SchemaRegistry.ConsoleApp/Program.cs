using System;
using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

using CommandLine;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaRegistry.ConsoleApp
{
    /// <summary>
    /// This represents the entity for the console app.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            var serialiserSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            var settings = new JsonSchemaGeneratorSettings()
            {
                SerializerSettings = serialiserSettings,
            };

            using (var parser = new Parser(with => { with.EnableDashDash = true; with.HelpWriter = Console.Out; }))
            {
                var result = parser.ParseArguments<Options>(args);
                result.WithParsed<Options>(async o =>
                {
                    var builder = new SchemaBuilder()
                                      .WithSettings(settings);

                    var schema = builder.Build<SampleClass>()
                                         .ToJson();

                    var sink = new FileSystemSchemaSink()
                                   .WithBaseLocation(o.BaseLocation);

                    var producer = new SchemaProducer()
                                       .WithBuilder(builder)
                                       .WithSink(sink);

                    var produced = await producer.ProduceAsync<SampleClass>(o.Filepath).ConfigureAwait(false);

                    var consumer = new SchemaConsumer()
                                       .WithSink(sink);

                    var downloaded = await consumer.ConsumeAsync(o.Filepath).ConfigureAwait(false);

                    Console.WriteLine("Schema Downloaded:");
                    Console.WriteLine(downloaded);
                });
            }
        }
    }
}
