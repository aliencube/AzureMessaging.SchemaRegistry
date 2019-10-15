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
    public class HttpClientSendExtensionsTests
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
                .Should().Contain(p => p.Name.IsEquivalentTo("SendAsync", StringComparison.CurrentCulture))
                ;
        }

        [DataTestMethod]
        [DataRow("GET")]
        [DataRow("POST")]
        [DataRow("PUT")]
        [DataRow("PATCH")]
        public void Given_Null_Parameters_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri))
            {
                func = async () => await HttpClientExtensions.SendAsync(null, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow("DELETE", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        public async Task Given_Unsupported_Method_When_SendAsync_Invoked_Then_It_Should_Return_405(string verb, HttpStatusCode statusCode, HttpStatusCode expected)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();

            var path = "default.json";

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            using (var response = await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path).ConfigureAwait(false))
            {
                response.StatusCode.Should().Be(expected);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.BadRequest)]
        [DataRow("POST", HttpStatusCode.BadRequest)]
        [DataRow("PUT", HttpStatusCode.BadRequest)]
        [DataRow("PATCH", HttpStatusCode.BadRequest)]
        public void Given_Error_Response_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.OK)]
        [DataRow("POST", HttpStatusCode.OK)]
        [DataRow("PUT", HttpStatusCode.OK)]
        [DataRow("PATCH", HttpStatusCode.OK)]
        public async Task Given_Validation_Result_When_SendAsync_Invoked_Then_It_Should_Return_Result(string verb, HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                var result = await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }

        [DataTestMethod]
        [DataRow("GET")]
        [DataRow("POST")]
        [DataRow("PUT")]
        [DataRow("PATCH")]
        public void Given_Null_Parameters_With_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var token = default(CancellationToken);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri))
            {
                func = async () => await HttpClientExtensions.SendAsync(null, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow("DELETE", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpStatusCode.MethodNotAllowed)]
        public async Task Given_Unsupported_Method_With_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Return_405(string verb, HttpStatusCode statusCode, HttpStatusCode expected)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();

            var path = "default.json";

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var response = await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path, source.Token).ConfigureAwait(false))
            {
                response.StatusCode.Should().Be(expected);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.BadRequest)]
        [DataRow("POST", HttpStatusCode.BadRequest)]
        [DataRow("PUT", HttpStatusCode.BadRequest)]
        [DataRow("PATCH", HttpStatusCode.BadRequest)]
        public void Given_Error_Response_With_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.OK)]
        [DataRow("POST", HttpStatusCode.OK)]
        [DataRow("PUT", HttpStatusCode.OK)]
        [DataRow("PATCH", HttpStatusCode.OK)]
        public async Task Given_Validation_Result_With_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Return_Result(string verb, HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                var result = await HttpClientExtensions.SendAsync(httpClient, request, validator.Object, path, source.Token).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("GET", HttpCompletionOption.ResponseContentRead)]
        [DataRow("POST", HttpCompletionOption.ResponseContentRead)]
        [DataRow("PUT", HttpCompletionOption.ResponseContentRead)]
        [DataRow("PATCH", HttpCompletionOption.ResponseContentRead)]
        public void Given_Null_Parameters_With_HttpCompletionOption_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri))
            {
                func = async () => await HttpClientExtensions.SendAsync(null, null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow("DELETE", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("DELETE", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        public async Task Given_Unsupported_Method_With_HttpCompletionOption_When_SendAsync_Invoked_Then_It_Should_Return_405(string verb, HttpStatusCode statusCode, HttpCompletionOption option, HttpStatusCode expected)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();

            var path = "default.json";

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            using (var response = await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path).ConfigureAwait(false))
            {
                response.StatusCode.Should().Be(expected);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("GET", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("POST", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("PUT", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("PATCH", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        public void Given_Error_Response_With_HttpCompletionOption_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        public async Task Given_Validation_Result_With_HttpCompletionOption_When_SendAsync_Invoked_Then_It_Should_Return_Result(string verb, HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                var result = await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("GET", HttpCompletionOption.ResponseContentRead)]
        [DataRow("POST", HttpCompletionOption.ResponseContentRead)]
        [DataRow("PUT", HttpCompletionOption.ResponseContentRead)]
        [DataRow("PATCH", HttpCompletionOption.ResponseContentRead)]
        public void Given_Null_Parameters_With_HttpCompletionOption_And_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var token = default(CancellationToken);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri))
            {
                func = async () => await HttpClientExtensions.SendAsync(null, null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow("DELETE", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("DELETE", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("HEAD", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("OPTIONS", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        [DataRow("TRACE", HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead, HttpStatusCode.MethodNotAllowed)]
        public async Task Given_Unsupported_Method_With_HttpCompletionOption_And_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Return_405(string verb, HttpStatusCode statusCode, HttpCompletionOption option, HttpStatusCode expected)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();

            var path = "default.json";

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var response = await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path, source.Token).ConfigureAwait(false))
            {
                response.StatusCode.Should().Be(expected);
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("GET", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("POST", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("PUT", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        [DataRow("PATCH", HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        public void Given_Error_Response_With_HttpCompletionOption_And_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Throw_Exception(string verb, HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var func = default(Func<Task>);
            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                func = async () => await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow("GET", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("POST", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PUT", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow("PATCH", HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        public async Task Given_Validation_Result_With_HttpCompletionOption_And_CancellationToken_When_SendAsync_Invoked_Then_It_Should_Return_Result(string verb, HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var request = this._fixture.CreateHttpRequestMessage(verb, requestUri, payload))
            {
                var result = await HttpClientExtensions.SendAsync(httpClient, request, option, validator.Object, path, source.Token).ConfigureAwait(false);
                result.Should().Be(response);
            }
        }
    }
}
