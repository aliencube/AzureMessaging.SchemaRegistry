using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorldDomination.Net.Http;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Tests
{
    [TestClass]
    public class HttpSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(HttpSchemaSink).Should().BeDerivedFrom<SchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(HttpSchemaSink)
                .Should().Implement<ISchemaSink>()
                .And.Implement<IHttpSchemaSink>()
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(HttpSchemaSink)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(Uri) })
                .And.HaveConstructor(new[] { typeof(HttpClient) })
                .And.HaveConstructor(new[] { typeof(string), typeof(HttpClient) })
                .And.HaveConstructor(new[] { typeof(Uri), typeof(HttpClient) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(HttpSchemaSink)
                .Should().HaveProperty<Encoding>("Encoding")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(HttpSchemaSink)
                .Should().HaveMethod("WithBaseLocation", new[] { typeof(Uri) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaSink>();

            typeof(HttpSchemaSink)
                .Should().HaveMethod("WithHttpClient", new[] { typeof(HttpClient) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaSink>();

            typeof(HttpSchemaSink)
                .Should().HaveMethod("GetSchemaAsync", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<string>>();

            typeof(HttpSchemaSink)
                .Should().HaveMethod("SetSchemaAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<bool>>();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Value()
        {
            var instance = new HttpSchemaSink();

            instance.BaseLocation.Should().BeEmpty();
            instance.Encoding.Should().Be(Encoding.UTF8);
        }

        [TestMethod]
        public void Given_Encoding_When_Instantiated_Then_It_Should_Return_Value()
        {
            var encoding = Encoding.ASCII;

            var instance = new HttpSchemaSink() { Encoding = encoding };

            instance.BaseLocation.Should().BeEmpty();
            instance.Encoding.Should().Be(encoding);
        }

        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            var action = default(Action);

            action = () => new HttpSchemaSink(location: (string)null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink(location: (Uri)null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink(httpClient: null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink((string)null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink((Uri)null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink("http://localhost", null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HttpSchemaSink(new Uri("http://localhost"), null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_Uri_When_WithBaseLocation_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Action action = () => instance.WithBaseLocation((Uri)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("http://localhost")]
        public void Given_Uri_When_WithBaseLocation_Invoked_Then_It_Should_Return_Result(string uri)
        {
            var instance = new HttpSchemaSink();

            var result = instance.WithBaseLocation(new Uri(uri));

            result.BaseLocation.Trim('/').Should().BeEquivalentTo(uri.Trim('/'));
        }

        [TestMethod]
        public void Given_Null_HttpClient_When_WithHttpClient_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Action action = () => instance.WithHttpClient(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_HttpClient_When_WithHttpClient_Invoked_Then_It_Should_Return_Result()
        {
            var httpClient = new HttpClient();
            var instance = new HttpSchemaSink();

            var result = instance.WithHttpClient(httpClient);

            var field = typeof(HttpSchemaSink).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);

            field.GetValue(instance).Should().Be(httpClient);

            httpClient.Dispose();
        }

        [TestMethod]
        public void Given_Null_Path_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Func<Task> func = async () => await instance.GetSchemaAsync(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_BaseLocation_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Func<Task> func = async () => await instance.GetSchemaAsync("hello-world").ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow("http://localhost/default.json", HttpStatusCode.BadRequest)]
        public void Given_Path_And_ErrorResponse_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string path, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithHttpClient(httpClient);

            Func<Task> func = async () => await instance.GetSchemaAsync(path).ConfigureAwait(false);

            func.Should().Throw<HttpRequestException>();

            handler.Dispose();
            httpClient.Dispose();
        }

        [DataTestMethod]
        [DataRow("http://localhost", "default.json", HttpStatusCode.BadRequest)]
        [DataRow("http://localhost", "http://localhost/default.json", HttpStatusCode.BadRequest)]
        public void Given_Location_And_Path_And_ErrorResponse_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string location, string path, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithBaseLocation(location)
                               .WithHttpClient(httpClient);

            Func<Task> func = async () => await instance.GetSchemaAsync(path).ConfigureAwait(false);

            func.Should().Throw<HttpRequestException>();

            handler.Dispose();
            httpClient.Dispose();
        }

        [DataTestMethod]
        [DataRow("http://localhost", "default.json", HttpStatusCode.OK, "{ \"hello\": \"world\" }")]
        [DataRow("http://localhost", "http://localhost/default.json", HttpStatusCode.OK, "{ \"hello\": \"world\" }")]
        public async Task Given_Location_And_Path_When_GetSchemaAsync_Invoked_Then_It_Should_Return_Result(string location, string path, HttpStatusCode statusCode, string schema)
        {
            var content = new StringContent(schema);
            var response = new HttpResponseMessage(statusCode) { Content = content };
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithBaseLocation(location)
                               .WithHttpClient(httpClient);

            var result = await instance.GetSchemaAsync(path).ConfigureAwait(false);

            result.Should().Be(schema);

            content.Dispose();
            handler.Dispose();
            httpClient.Dispose();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Func<Task> func = async () => await instance.SetSchemaAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.SetSchemaAsync("hello-world", null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_BaseLocation_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new HttpSchemaSink();

            Func<Task> func = async () => await instance.SetSchemaAsync("hello-world", "default.json").ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "http://localhost/default.json", HttpStatusCode.BadRequest)]
        public void Given_Schema_And_Path_And_ErrorResponse_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string schema, string path, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithHttpClient(httpClient);

            Func<Task> func = async () => await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            func.Should().Throw<HttpRequestException>();

            handler.Dispose();
            httpClient.Dispose();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "http://localhost", "default.json", HttpStatusCode.BadRequest)]
        [DataRow("{ \"hello\": \"world\" }", "http://localhost", "http://localhost/default.json", HttpStatusCode.BadRequest)]
        public void Given_Location_And_Path_And_ErrorResponse_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string schema, string location, string path, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithBaseLocation(location)
                               .WithHttpClient(httpClient);

            Func<Task> func = async () => await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            func.Should().Throw<HttpRequestException>();

            handler.Dispose();
            httpClient.Dispose();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "http://localhost", "default.json", HttpStatusCode.OK)]
        [DataRow("{ \"hello\": \"world\" }", "http://localhost", "http://localhost/default.json", HttpStatusCode.OK)]
        public async Task Given_Location_And_Path_When_SetSchemaAsync_Invoked_Then_It_Should_Return_Result(string schema, string location, string path, HttpStatusCode statusCode)
        {
            var content = new StringContent(schema);
            var response = new HttpResponseMessage(statusCode) { Content = content };
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var instance = new HttpSchemaSink()
                               .WithBaseLocation(location)
                               .WithHttpClient(httpClient);

            var result = await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            result.Should().BeTrue();

            content.Dispose();
            handler.Dispose();
            httpClient.Dispose();
        }
    }
}
