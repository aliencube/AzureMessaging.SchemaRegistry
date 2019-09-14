# Schema Validation via HTTP Client #

This is a library containing a collection of extension methods of `HttpClient` for schema validation. Schema registry is accessible through [SchemaValidator](./schema-validation.md).


## NuGet Package Status ##

| Package | Download | Version|
|---|---|---|
| [Aliencube.AzureMessaging.SchemaValidation.HttpClient](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.HttpClient/) | [![](https://img.shields.io/nuget/dt/Aliencube.AzureMessaging.SchemaValidation.HttpClient.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.HttpClient/) | [![](https://img.shields.io/nuget/v/Aliencube.AzureMessaging.SchemaValidation.HttpClient.svg)](https://www.nuget.org/packages/Aliencube.AzureMessaging.SchemaValidation.HttpClient/) |


## Usage ##

### HTTP: GET ###

There are ten extension methods around HTTP GET:

* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri, HttpCompletionOption completionOption, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<string> GetStringAsync(this HttpClient httpClient, string requestUri, ISchemaValidator validator, string path)`
* `Task<string> GetStringAsync(this HttpClient httpClient, Uri requestUri, ISchemaValidator validator, string path)`

Their usages are almost identical to each other. All needs both `ISchemaValidator` instance and `path` value for validation. After message is received through the HTTP `GET` request, the payload is then validated by the validator. The sample code below is a REST API request to one of Azure messaging service to pick up an event/message payload, then validate it.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var path = $"v1/schema.json";

var requestUri = "https://my-azure-messaging-service.com/messages/123";
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer xxx");

var payload = await httpClient.GetStringAsync(requestUri, validator, path)
                              .ConfigureAwait(false);
```


### HTTP: POST ###

There are four extension methods around HTTP POST:

* `Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`

Their usages are almost identical to each other. All needs both `ISchemaValidator` instance and `path` value for validation. Before message is sent through the HTTP `POST` request, the payload is validated by the validator. The sample code below is a REST API request to one of Azure messaging services.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var path = $"v1/schema.json";

var payload = "{ \"hello\": \"world\" }";
var content = new StringContent(payload);

var requestUri = "https://my-azure-messaging-service.com/messages";
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer xxx");

var response = await httpClient.PostAsync(requestUri, content, validator, path)
                               .ConfigureAwait(false);
```


### HTTP: PUT ###

There are four extension methods around HTTP PUT:

* `Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`

Their usages are almost identical to each other. All needs both `ISchemaValidator` instance and `path` value for validation. Before message is sent through the HTTP `PUT` request, the payload is validated by the validator. The sample code below is a REST API request to one of Azure messaging services.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var path = $"v1/schema.json";

var payload = "{ \"hello\": \"world\" }";
var content = new StringContent(payload);

var requestUri = "https://my-azure-messaging-service.com/messages/123";
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer xxx");

var response = await httpClient.PutAsync(requestUri, content, validator, path)
                               .ConfigureAwait(false);
```


### HTTP: PATCH ###

There are four extension methods around HTTP PATCH:

* `Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Uri requestUri, HttpContent content, ISchemaValidator validator, string path, CancellationToken cancellationToken)`

Their usages are almost identical to each other. All needs both `ISchemaValidator` instance and `path` value for validation. Before message is sent through the HTTP `PATCH` request, the payload is validated by the validator. The sample code below is a REST API request to one of Azure messaging services.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var path = $"v1/schema.json";

var payload = "{ \"hello\": \"world\" }";
var content = new StringContent(payload);

var requestUri = "https://my-azure-messaging-service.com/messages/123";
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer xxx");

var response = await httpClient.PatchAsync(requestUri, content, validator, path)
                               .ConfigureAwait(false);
```


### HTTP: SEND ###

There are four extension methods around generic HTTP send:

* `Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, string requestUri, HttpRequestMessage request, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, string requestUri, HttpRequestMessage request, ISchemaValidator validator, string path, CancellationToken cancellationToken)`
* `Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Uri requestUri, HttpRequestMessage request, ISchemaValidator validator, string path)`
* `Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Uri requestUri, HttpRequestMessage request, ISchemaValidator validator, string path, CancellationToken cancellationToken)`

Their usages are almost identical to each other. All needs both `ISchemaValidator` instance and `path` value for validation. When a request is made, depending on the request verb, the message is validated before being sent (`POST`, `PUT`, `PATCH`) or after being received (`GET`) by the validator. The sample code below is a REST API request to one of Azure messaging services.

```csharp
var sink = new BlobStorageSchemaSink();
var consumer = new SchemaConsumer()
                   .WithSink(sink);

var validator = new SchemaValidator()
                    .WithSchemaConsumer(consumer);

var path = $"v1/schema.json";

var payload = "{ \"hello\": \"world\" }";
var content = new StringContent(payload);

var requestUri = "https://my-azure-messaging-service.com/messages/123";

var method = HttpMethod.Post;
var request = new HttpRequestMessage(method, requestUri);
request.Content = content;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer xxx");

var response = await httpClient.SendAsync(requestUri, request, validator, path)
                               .ConfigureAwait(false);
```
