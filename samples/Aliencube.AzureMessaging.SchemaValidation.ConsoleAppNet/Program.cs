using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Extensions;
using Aliencube.AzureMessaging.SchemaValidation.ServiceBus;

using CommandLine;

using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaValidation.ConsoleAppNet
{
    /// <summary>
    /// This represents the entity for the console app.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
    [SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler")]
    public static class Program
    {
        private const string SchemaRegistered = "Schema registered";
        private const string MessageSent = "Message sent";
        private const string MessageReceived = "Message received";

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
            await RegisterSchemaAsync<SampleClass>(options).ConfigureAwait(false);

            var payload = new SampleClass();

            await SendMessageAsync(options, payload)
                  .ConfigureAwait(false);

            await ReceiveMessageAsync(options)
                  .ConfigureAwait(false);
        }

        private static ISchemaSink GetSchemaSink(Options options)
        {
            var blobClient = CloudStorageAccount.Parse(options.BlobConnectionString)
                                                .CreateCloudBlobClient();

            var sink = new BlobStorageSchemaSink(blobClient)
                           .WithBaseLocation(options.BlobBaseUri)
                           .WithContainer(options.Container);

            return sink;
        }

        private static ISchemaConsumer GetSchemaConsumer(Options options)
        {
            var sink = GetSchemaSink(options);

            var consumer = new SchemaConsumer()
                               .WithSink(sink);

            return consumer;
        }

        private static async Task RegisterSchemaAsync<T>(Options options)
        {
            var sink = GetSchemaSink(options);

            var builder = new SchemaBuilder()
                              .WithSettings(Settings);

            var schema = builder.Build<T>()
                                .ToJson();

            var producer = new SchemaProducer()
                               .WithBuilder(builder)
                               .WithSink(sink);

            var produced = await producer.ProduceAsync<T>(options.Filepath)
                                         .ConfigureAwait(false);

            Console.WriteLine(SchemaRegistered);
        }

        private static async Task SendMessageAsync<T>(Options options, T payload)
        {
            var consumer = GetSchemaConsumer(options);

            var validator = new SchemaValidator()
                                .WithSchemaConsumer(consumer);

            var factory = MessagingFactory.CreateFromConnectionString(options.ServiceBusConnectionString);
            var serialised = JsonConvert.SerializeObject(payload, Settings.SerializerSettings);
            var body = Encoding.UTF8.GetBytes(serialised);
            using (var stream = new MemoryStream(body))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", $"{options.BlobBaseUri.TrimEnd('/')}/{options.Container.Trim('/')}/{options.Filepath.TrimStart('/')}");

                var topic = factory.CreateTopicClient(options.Topic);

                var validated = await message.ValidateAsync(validator)
                                             .ConfigureAwait(false);

                await topic.SendAsync(validated)
                           .ConfigureAwait(false);
            }

            Console.WriteLine(MessageSent);
        }

        private static async Task ReceiveMessageAsync(Options options)
        {
            var consumer = GetSchemaConsumer(options);

            var validator = new SchemaValidator()
                                .WithSchemaConsumer(consumer);

            var factory = MessagingFactory.CreateFromConnectionString(options.ServiceBusConnectionString);

            var subscription = factory.CreateSubscriptionClient(options.Topic, options.Subscription, ReceiveMode.PeekLock);

            var received = await subscription.ReceiveAsync()
                                             .ValidateAsync(validator)
                                             .ConfigureAwait(false);

            Console.WriteLine($"MessageId: {received.MessageId}, SequenceNumber: {received.SequenceNumber}");

            var payload = default(string);
            using (var stream = received.GetBody<Stream>())
            using (var reader = new StreamReader(stream))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            Console.WriteLine(payload);

            await subscription.CompleteAsync(received.LockToken)
                              .ConfigureAwait(false);

            Console.WriteLine(MessageReceived);
        }
    }
}
