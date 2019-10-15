# Schema Registry Sink for Azure Blob Storage #

This library provides a sink class to Azure Blob Storage that is used for the schema registry.


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob/) |


## Usage ##

### `IBlobStorageSchemaSink` and `BlobStorageSchemaSink` ###

As an extension of `ISchemaSink` and `SchemaSink` respectively, both `IBlobStorageSchemaSink` and `BlobStorageSchemaSink` declare extra property and methods:

* `Container`: Declares the blob container name.
* `WithBlobClient(CloudBlobClient blobClient)`: Adds the Azure Blob Storage client instance.
* `WithContainer(string container)`: Adds the blob container name.

It also has two overriding methods, `GetSchemaAsync(string path)` and `SetSchemaAsync(string schema, string path)`.

> **NOTE**: The `path` parameter value **MAY** be fully qualified URL. However, if it is the fully qualified URL path, both methods rip off both base location and container name from it. Make sure that, when the fully qualified URL is used, it **MUST** contain the same base URL and container name as the sink recognises. Otherwise it will throw an exception.

```csharp
var connectionString = "UseDevelopmentStorage=true";
var container = "schemas";
var account = CloudStorageAccount.Parse(connectionString);
var blobClient = account.CreateCloudBlobClient();

var sink = new BlobStorageSchemaSink()
               .WithBaseLocation(blobClient.BaseUri)
               .WithBlobClient(blobClient)
               .WithContainer(container);

var schema = "{" +
             "  \"type\": \"object\"," +
             "  \"properties\": {" +
             "    \"hello\": {" +
             "      \"type\": \"string\"" +
             "    }" +
             "  }" +
             "}";

var path1 = "v1/schema.json";

var sinked1 = await sink.SetSchemaAsync(schema, path1)
                        .ConfigureAwait(false);

var schema1 = await sink.GetSchemaAsync(path1)
                        .ConfigureAwait(false);

var path2 = "https://my-schema-registry.blob.core.windows.net/schemas/v1/schema.json";

var sinked2 = await sink.SetSchemaAsync(schema, path2)
                        .ConfigureAwait(false);

var schema2 = await sink.GetSchemaAsync(path2)
                        .ConfigureAwait(false);
```
