# Schema Registry Sink for File System #

This library provides a sink class to local file system that is used for the schema registry.


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem/) |


## Usage ##

### `IFileSystemSchemaSink` and `FileSystemSchemaSink` ###

As an extension of `ISchemaSink` and `SchemaSink` respectively, both `IFileSystemSchemaSink` and `FileSystemSchemaSink` declare extra properties:

* `Directory`: Gets or sets the `IDirectoryWrapper` instance. Default is `DirectoryWrapper`.
* `File`: Gets or sets the `IFileWrapper` instance. Default is `FileWrapper`.
* `Encoding`: Gets or sets the file encoding. Default is `Encoding.UTF8`.

It also has two overriding methods, `GetSchemaAsync(string path)` and `SetSchemaAsync(string schema, string path)`.

```csharp
var location = "/etc/schema-registry/";

var sink = new FileSystemSchemaSink()
               .WithBaseLocation(location);

var schema = "{" +
             "  \"type\": \"object\"," +
             "  \"properties\": {" +
             "    \"hello\": {" +
             "      \"type\": \"string\"" +
             "    }" +
             "  }" +
             "}";

var path = "v1/schema.json";

var sinked = await sink.SetSchemaAsync(schema, path)
                       .ConfigureAwait(false);

var schema = await sink.GetSchemaAsync(path)
                       .ConfigureAwait(false);
```
