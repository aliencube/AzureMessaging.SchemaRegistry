# Schema Registry #

This is a schema registry library to produce a JSON schema and upload it to a repository, or download the JSON schema from the registry and consume it. Schema registry is accessible through [SchemaSink](./schema-registry-sinks.md).


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry/) |


## Usage ##

### `SchemaProducer` ###

`SchemaProducer` creates a JSON schema and uploads it to given sinks. Currently, there are three official sinks available:

* [`BlobStorageSchemaSink`](./schema-registry-sinks-blob.md)
* [`FileSystemSchemaSink`](./schema-registry-sinks-file-system.md)
* [`HttpSchemaSink`](./docs/schema-registry-sinks-http.md)

You can register as many sinks as you want, if you plan to duplicate sink locations. The following sample code shows how `SchemaProducer` uploads the schema through three different sinks.

```csharp
var settings = new JsonSchemaGeneratorSettings();
var builder = new SchemaBuilder().WithSettings(settings);

var sink1 = new FileSystemSchemaSink();
var sink2 = new HttpSchemaSink();
var sink3 = new BlobStorageSchemaSink();

var version = "v1";
var filename = "schema.json";
var path = $"{version}/{filename}";

var producer = new SchemaProducer()
                   .WithBuilder(builder)
                   .WithSink(sink1)
                   .WithSink(sink2)
                   .WithSink(sink3);

var produced = await producer.ProduceAsync<SampleClass>(path).ConfigureAwait(false);
```

> **Note**: This code deliberately omits how each sink is configured. For more details how it works, go to each sink page.


### `SchemaConsumer` ###

`SchemaConsumer` downloads a JSON schema from the given sink. Unlike `SchemaProducer`, it can only uses one `SchemaSink` at one time.

```csharp
var sink = new BlobStorageSchemaSink();

var version = "v1";
var filename = "schema.json";
var path = $"{version}/{filename}";

var consumer = new SchemaConsumer()
                   .WithSink(sink);

var schema = await consumer.ConsumeAsync(path).ConfigureAwait(false);
```

> **Note**: This code deliberately omits how each sink is configured. For more details on how it works, go to each sink page.
