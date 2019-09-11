# AzureMessaging.SchemaRegistry #

This provides libraries to enable schema registry features for Azure messaging services, including Azure Service, Bus, Azure Event Hub and Azure Event Grid.


## Build Status ##

[![Build Status](https://dev.azure.com/aliencube/AzureMessaging.SchemaRegistry/_apis/build/status/dev%2C%20feature%2C%20hotfix?branchName=dev)](https://dev.azure.com/aliencube/AzureMessaging.SchemaRegistry/_build/latest?definitionId=8&branchName=dev)


## List of NuGet Packages ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaRegistry](./docs/schema-registry.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry/) |
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks](./docs/schema-registry-sinks.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks/) |
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob](./docs/schema-registry-sinks-blob.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob/) |
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem](./docs/schema-registry-sinks-file-system.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem/) |
| [Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http](./docs/schema-registry-sinks-http.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http/) |
| [Aliencube.AzureMessaging.SchemaValidation](./docs/schema-validation.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation/) |
| [Aliencube.AzureMessaging.SchemaValidation.HttpClient](./docs/schema-validation-http-client.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.HttpClient.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.HttpClient/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.HttpClient.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.HttpClient/) |
| [Aliencube.AzureMessaging.SchemaValidation.ServiceBus](./docs/schema-validation-service-bus.md) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.ServiceBus.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.ServiceBus/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.ServiceBus.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.ServiceBus/) |


## Sample Codes ##

* [Schema Registry Console App](./samples/Aliencube.AzureMessaging.SchemaRegistry.ConsoleApp)
* [Schema Validation Azure Function App (v1)](./samples/Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV1)
* [Schema Validation Azure Function App (v2)](./samples/Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV2)
* [Schema Validation Console App (.NET Framework)](./samples/Aliencube.AzureMessaging.SchemaValidation.ConsoleAppCore)
* [Schema Validation Console App (.NET Core)](./samples/Aliencube.AzureMessaging.SchemaValidation.ConsoleAppNet)


## Contribution ##

Your contributions are always welcome! All your work should be done in your forked repository. Once you finish your work with corresponding tests, please send us a pull request onto our `dev` branch for review.


## License ##

**AzureMessaging.SchemaRegistry** is released under [MIT License](http://opensource.org/licenses/MIT)

> The MIT License (MIT)
>
> Copyright (c) 2019 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
