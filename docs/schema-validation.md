# Schema Validation #

This is a schema validation library that consumes a JSON schema from the schema registry and validate event/message payload against the schema. Schema registry is accessible through [SchemaConsumer](./schema-registry.md#schemaconsumer).


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaValidation](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation/) |


## Usage ##

### `ISchemaValidator` and `SchemaValidator` ###

As an interface, and as an abstract class, both `ISchemaValidator` and `SchemaValidator` declare the following properties and methods:

* `Consumer`: Declares the `ISchemaConsumer` instance.
* `WithSchemaConsumer(ISchemaConsumer consumer)`: Adds the `ISchemaConsumer` instance to the validator.
* `ValidateAsync(string payload, string path)`: Validates the payload against the schema through the `SchemaConsumer` instance.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var payload = "{ \"hello\": \"world\" }";
var path = $"v1/schema.json";

var validated = await validator.ValidateAsync(payload, path)
                               .ConfigureAwait(false);
```


### Schema Validation Extension ###

There is an extension method, `StringExtensions.ValidateAsync(this string payload, ISchemaValidator validator, string path)` that directly applies to the event/message payload. It is particularly useful for Azure Function triggers. The following function code is to apply the validation for Azure Service Bus, but it can be applied to any message triggers.

```csharp
public static class ServiceBusTopicSubscriptionTrigger
{
    [FunctionName(nameof(ServiceBusTopicSubscriptionTrigger))]
    public static async Task Run(
        [ServiceBusTrigger(/* OMITTED */)] string message,
        ILogger log)
    {
        var path = "default.json";

        var sink = new BlobStorageSchemaSink();
        var consumer = new SchemaConsumer()
                           .WithSink(sink);
        var validator = new SchemaValidator()
                            .WithSchemaConsumer(consumer);

        var validated = await message.ValidateAsync(validator, path)
                                     .ConfigureAwait(false);
    }
}
```

There is another extension method, `StringExtensions.ValidateAsStringAsync(this string payload, ISchemaValidator validator, string path)` that directly applies to the event/message payload. It is particularly useful for Azure Function outputs. The following function code is to apply the validation for Azure Service Bus, but it can be applied to any message output bindings.

```csharp
public static class ServiceBusTopicPublishHttpTrigger
{
    [FunctionName(nameof(ServiceBusTopicPublishHttpTrigger))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(/* OMITTED */)] HttpRequest req,
        [ServiceBus(/* OMITTED */)] IAsyncCollector<string> collector,
        ILogger log)
    {
        var path = "default.json";
        var sink = new BlobStorageSchemaSink();
        var consumer = new SchemaConsumer()
                           .WithSink(sink);
        var validator = new SchemaValidator()
                            .WithSchemaConsumer(consumer);

        var sample = new SampleClass();

        var payload = await JsonConvert.SerializeObject(sample)
                                       .ValidateAsStringAsync(validator, path)
                                       .ConfigureAwait(false);

        await collector.AddAsync(payload)
                       .ConfigureAwait(false);

        return new StatusCodeResult((int)HttpStatusCode.Created);
    }
}
```
