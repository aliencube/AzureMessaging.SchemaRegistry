# Schema Registry Sink #

This library provides an interface (`ISchemaSink`) and an abstract class (`SchemaSink`) so that other third-party sink providers follow the same design principles.

Currently, there are three official sinks available:

* [`BlobStorageSchemaSink`](./schema-registry-sinks-blob.md)
* [`FileSystemSchemaSink`](./schema-registry-sinks-file-system.md)
* [`HttpSchemaSink`](./docs/schema-registry-sinks-http.md)


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) |
