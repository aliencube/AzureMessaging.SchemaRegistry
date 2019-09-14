# Schema Registry Sink for File System #

This library provides a sink class to the schema registry over the HTTP connection.


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http/) |


## Usage ##

### `IHttpSchemaSink` and `HttpSchemaSink` ###

As an extension of `ISchemaSink` and `SchemaSink` respectively, both `IHttpSchemaSink` and `HttpSchemaSink` declare extra properties:

* `Encoding`: Gets or sets the file encoding. Default is `Encoding.UTF8`.
* `WithHttpClient(HttpClient httpClient)`: Adds the `HttpClient` instance.

It also has two overriding methods, `GetSchemaAsync(string path)` and `SetSchemaAsync(string schema, string path)`.

> **NOTE**: The `path` parameter value **MAY** be fully qualified URL. However, if it is the fully qualified URL path, both methods rip off the base location from it. Make sure that, when the fully qualified URL is used, it **MUST** contain the same base URL as the sink recognises. Otherwise it will throw an exception.

```csharp
var location = "https://my-schema-registry.net/schemas/";
var httpClient = new HttpClient();

var sink = new HttpSchemaSink()
               .WithBaseLocation(location)
               .WithHttpClient(httpClient);

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

var path2 = "https://my-schema-registry.net/schemas/v1/schema.json";

var sinked2 = await sink.SetSchemaAsync(schema, path2)
                        .ConfigureAwait(false);

var schema2 = await sink.GetSchemaAsync(path2)
                        .ConfigureAwait(false);
```
