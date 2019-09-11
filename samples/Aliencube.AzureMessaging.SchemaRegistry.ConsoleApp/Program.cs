using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

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
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    [SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler")]
    public static class Program
    {
        private const string SchemaProduced = "Schema produced to registry";
        private const string SchemaConsumed = "Schema consumed from registry";

        private static JsonSchemaGeneratorSettings Settings { get; } =
            new JsonSchemaGeneratorSettings()
            {
                SerializerSettings =
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                    }
            };

        /// <summary>
        /// Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            using (var parser = new Parser(with => { with.EnableDashDash = true; with.HelpWriter = Console.Out; }))
            {
                var result = parser.ParseArguments<Options>(args);
                result.WithParsed<Options>(async options => await ProcessAsync(options).ConfigureAwait(false));
            }

            Console.ReadLine();
        }

        private static async Task ProcessAsync(Options options)
        {
            var builder = new SchemaBuilder()
                              .WithSettings(Settings);

            var schema = builder.Build<SampleClass>()
                                 .ToJson();

            var sink = new FileSystemSchemaSink()
                           .WithBaseLocation(options.BaseLocation);

            var producer = new SchemaProducer()
                               .WithBuilder(builder)
                               .WithSink(sink);

            var produced = await producer.ProduceAsync<SampleClass>(options.Filepath).ConfigureAwait(false);

            Console.WriteLine(SchemaProduced);

            var consumer = new SchemaConsumer()
                               .WithSink(sink);

            var downloaded = await consumer.ConsumeAsync(options.Filepath).ConfigureAwait(false);

            Console.WriteLine(downloaded);
            Console.WriteLine(SchemaConsumed);
        }
    }
}
