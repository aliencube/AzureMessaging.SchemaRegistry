using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;
using Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests.Fixture;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests
{
    [TestClass]
    public class HttpClientPatchExtensionsTests
    {
        private HttpClientExtensionsFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            this._fixture = new HttpClientExtensionsFixture();
        }

        [TestCleanup]
        public void TearDown()
        {
            this._fixture.Dispose();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(HttpClientExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name.IsEquivalentTo("PatchAsync", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var content = this._fixture.CreateHttpContent())
            {
                func = async () => await HttpClientExtensions.PatchAsync(null, (string)null, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, (string)null, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(null, (Uri)null, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, (Uri)null, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public void Given_Validation_Error_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        public void Given_Error_Response_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public async Task Given_Validation_Result_When_PatchAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            {
                var result = await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path).ConfigureAwait(false);
                result.Should().Be(response);
            }

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            {
                var result = await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }

        [TestMethod]
        public void Given_Null_Parameters_With_CancellationToken_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";
            var token = default(CancellationToken);

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var content = this._fixture.CreateHttpContent())
            {
                func = async () => await HttpClientExtensions.PatchAsync(null, (string)null, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, (string)null, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(null, (Uri)null, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, (Uri)null, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public void Given_Validation_Error_With_CancellationToken_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        public void Given_Error_Response_With_CancellationToken_When_PatchAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public async Task Given_Validation_Result_With_CancellationToken_When_PatchAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                var result = await HttpClientExtensions.PatchAsync(httpClient, requestUri, content, validator.Object, path, source.Token).ConfigureAwait(false);
                result.Should().Be(response);
            }

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var content = this._fixture.CreateHttpContent())
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                var result = await HttpClientExtensions.PatchAsync(httpClient, new Uri(requestUri), content, validator.Object, path, source.Token).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }
    }
}
