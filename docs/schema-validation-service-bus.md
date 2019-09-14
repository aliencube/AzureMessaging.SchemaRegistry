# Schema Validation via Azure Service Bus #

This is a library to validate schema when a message is sent to or received from Azure Service Bus SDK. Schema registry is accessible through [SchemaValidator](./schema-validation.md).


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaValidation.ServiceBus](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.ServiceBus/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.ServiceBus.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.ServiceBus/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.ServiceBus.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.ServiceBus/) |


## Usage ##

Due to the fact that there are two different version of Azure Service Bus SDK, each version takes a significantly different approach from each other, to validate messages.


### `BrokeredMessage` Extension ###

> **NOTE**: When your application targets .NET Framework (4.6.1+) and uses the NuGet package of [`WindowsAzure.ServiceBus`](https://www.nuget.org/packages/WindowsAzure.ServiceBus/), this approach **SHOULD** be taken.

Before sending a message through `TopicClient`, the message is validated by this extension method, `ValidateAsync(this BrokeredMessage message, ISchemaValidator validator, string schemaPathPropertyKey = "schemaPath")`. Make sure that the message **MUST** include a user property pointing to the schema location.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var factory = MessagingFactory.CreateFromConnectionString("/* CONNECTION STRING */");
var topic = factory.CreateTopicClient("my-topic");

var payload = "{ \"hello\": \"world\" }";
var body = Encoding.UTF8.GetBytes(payload);
using (var stream = new MemoryStream(body))
using (var message = new BrokeredMessage(stream))
{
    message.Properties.Add("schemaPath", "https://my-schema-registry.blob.core.windows.net/schemas/v1/schema.json");

    var validated = await message.ValidateAsync(validator)
                                 .ConfigureAwait(false);

    await topic.SendAsync(validated)
               .ConfigureAwait(false);
}
```

After receiving the message through `SubscriptionClient`, the message is validated by this extension method, `ValidateAsync(this BrokeredMessage message, ISchemaValidator validator, string schemaPathPropertyKey = "schemaPath")`. Make sure that the message **MUST** include a user property pointing to the schema location.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var factory = MessagingFactory.CreateFromConnectionString("/* CONNECTION STRING */");
var subscription = factory.CreateSubscriptionClient("my-topic", "my-subscription", ReceiveMode.PeekLock);

var received = await subscription.ReceiveAsync()
                                 .ValidateAsync(validator)
                                 .ConfigureAwait(false);
```


### `SchemaValidatorPlugin` Extending `ServiceBusPlugin` ###

> **NOTE**: When you application targets .NET Core (2.1+) and uses the NuGet package of [`Microsoft.Azure.ServiceBus`](https://www.nuget.org/packages/Microsoft.Azure.ServiceBus/), this approach **SHOULD** be taken.

Register `SchemaValidatorPlugin` to `TopicClient`. When a message is sent to through the `TopicClient`, the message is validated by `SchemaValidatorPlugin`. Make sure that the message **MUST** include a user property pointing to the schema location.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var plugin = new SchemaValidatorPlugin()
                 .WithValidator(validator);

var topic = new TopicClient("/* CONNECTION STRING */", "my-topic");
topic.RegisterPlugin(plugin);

var payload = "{ \"hello\": \"world\" }";
var body = Encoding.UTF8.GetBytes(payload);
var message = new Message(body);
message.UserProperties.Add("schemaPath", "https://my-schema-registry.blob.core.windows.net/schemas/v1/schema.json");

await topic.SendAsync(message)
           .ConfigureAwait(false);
```

Register `SchemaValidatorPlugin` to `SubscriptionClient`. When a message is received to through the `SubscriptionClient`, the message is validated by `SchemaValidatorPlugin`. Make sure that the message **MUST** include a user property pointing to the schema location.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var plugin = new SchemaValidatorPlugin()
                 .WithValidator(validator);

var subscription = new SubscriptionClient("/* CONNECTION STRING */", "my-topic", "my-subscription", ReceiveMode.PeekLock);
subscription.RegisterPlugin(plugin);
subscription.RegisterMessageHandler(async (message, token) =>
{
    var payload = Encoding.UTF8.GetString(message.Body);

    await subscription.CompleteAsync(message.SystemProperties.LockToken)
                      .ConfigureAwait(false);
},
new MessageHandlerOptions(args =>
{
    Console.WriteLine(args.Exception.Message);
    Console.WriteLine(args.ExceptionReceivedContext.EntityPath);
}));
```
