# Schema Registry Sink #

This library provides an interface (`ISchemaSink`) and an abstract class (`SchemaSink`) for schema registry so that other third-party sink providers follow the same design principles.

Currently, there are three official sinks available:

* [`BlobStorageSchemaSink`](./schema-registry-sinks-blob.md)
* [`FileSystemSchemaSink`](./schema-registry-sinks-file-system.md)
* [`HttpSchemaSink`](./docs/schema-registry-sinks-http.md)


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) |


## Considerations ##

When you plan to build a new sink, by extending `SchemaSink`, the new sink **SHOULD** target both `net461` and `netestandard2.0`.


## Usage ##

### `ISchemaSink` and `SchemaSink` ###

As an interface, and as an abstract class, both `ISchemaSink` and `SchemaSink` declare the following properties and methods:

* `BaseLocation`: Declares the base location where the sink is located. Default is `string.Empty`.
* `WithBaseLocation(string location)`: Adds the base location where the sink is located.
* `GetSchemaAsync(string path)`: Gets the JSON schema from the schema registry.
* `SetSchemaAsync(string schema, string path)`: Sets the JSON schema to the given path in the schema registry.

As `SchemaSink` is an abstract class, this **MUST** be inherited by other sinks. There are a few considerations when extending `SchemaSink`:

* `SchemaSink` has two constructors &ndash; one without parameters and the other with `string location` that takes the base location of the sink. Therefore,
  * A new sink extending `SchemaSink` **SHOULD** implement both constructors, or
  * The new sink **MUST** implement at least the one with the `string location` parameter.
* The new sink extending `SchemaSink` **SHOULD** override both `GetSchemaAsync(string)` and `SetSchemaAsync(string, string)` for further processing.

```csharp
public class FakeSchemaSink : SchemaSink
{
    public FakeSchemaSink()
    {
    }

    public FakeSchemaSink(string location)
        : base(location)
    {
    }

    public override async Task<string> GetSchemaAsync(string path)
    {
        // Implement logic here

        return schema;
    }

    public override async Task<bool> SetSchemaAsync(string schema, string path)
    {
        // Implement logic here

        return true;
    }
}
```
