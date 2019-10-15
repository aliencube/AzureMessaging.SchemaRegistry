using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Extensions;

using CommandLine;

using Microsoft.WindowsAzure.Storage;

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

            var fileSink = GetFileSchemaSink(options);
            var blobSink = GetBlobSchemaSink(options);

            await ProduceAsync(builder, options, fileSink, blobSink)
                  .ConfigureAwait(false);

            await ConsumeAsync(options, blobSink)
                  .ConfigureAwait(false);

            await ConsumeAsync(options, fileSink)
                  .ConfigureAwait(false);
        }

        private static ISchemaSink GetFileSchemaSink(Options options)
        {
            var sink = new FileSystemSchemaSink()
                           .WithBaseLocation(options.FileBaseLocation);

            return sink;
        }

        private static ISchemaSink GetBlobSchemaSink(Options options)
        {
            var blobClient = CloudStorageAccount.Parse(options.BlobConnectionString)
                                                .CreateCloudBlobClient();

            var sink = new BlobStorageSchemaSink(blobClient)
                           .WithBaseLocation(options.BlobBaseUri)
                           .WithContainer(options.Container);

            return sink;
        }

        private static async Task ProduceAsync(ISchemaBuilder builder, Options options, params ISchemaSink[] sinks)
        {
            var producer = new SchemaProducer()
                               .WithBuilder(builder);

            foreach (var sink in sinks)
            {
                producer.WithSink(sink);
            }

            var produced = await producer.ProduceAsync<SampleClass>(options.Filepath).ConfigureAwait(false);

            foreach (var sink in sinks)
            {
                Console.WriteLine($"{sink.Name}: {SchemaProduced}");
            }
        }

        private static async Task ConsumeAsync(Options options, ISchemaSink sink)
        {
            var consumer = new SchemaConsumer()
                               .WithSink(sink);

            var downloaded = await consumer.ConsumeAsync(options.Filepath).ConfigureAwait(false);

            Console.WriteLine($"{sink.Name}: {SchemaConsumed}");
            Console.WriteLine(downloaded);
        }
    }
}
