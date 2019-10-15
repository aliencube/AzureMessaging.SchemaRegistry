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
    public class HttpClientGetExtensionsTests
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
                .Should().Contain(p => p.Name.IsEquivalentTo("GetAsync", StringComparison.CurrentCulture))
                .And.Contain(p => p.Name.IsEquivalentTo("GetStringAsync", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_GetAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            {
                func = async () => await HttpClientExtensions.GetAsync(null, (string)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (string)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(null, (Uri)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (Uri)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        public void Given_Error_Response_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public void Given_Validation_Error_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
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
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public async Task Given_Validation_Result_When_GetAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var result1 = await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false))
            using (var result2 = await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false))
            {
                result1.Should().Be(response);
                result2.Should().Be(response);
            }
        }

        [TestMethod]
        public void Given_Null_Parameters_With_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";
            var token = default(CancellationToken);

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            {
                func = async () => await HttpClientExtensions.GetAsync(null, (string)null, (ISchemaValidator)null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (string)null, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(null, (Uri)null, (ISchemaValidator)null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (Uri)null, (ISchemaValidator)null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), (ISchemaValidator)null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        public void Given_Error_Response_With_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public void Given_Validation_Error_With_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
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
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [TestMethod]
        public async Task Given_Validation_Result_With_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Return_Result()
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(HttpStatusCode.OK, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var result1 = await HttpClientExtensions.GetAsync(httpClient, requestUri, validator.Object, path, source.Token).ConfigureAwait(false))
            using (var result2 = await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), validator.Object, path, source.Token).ConfigureAwait(false))
            {
                result1.Should().Be(response);
                result2.Should().Be(response);
            }
        }

        [DataTestMethod]
        [DataRow(HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpCompletionOption.ResponseContentRead)]
        public void Given_Null_Parameters_With_HttpCompletionOption_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            {
                func = async () => await HttpClientExtensions.GetAsync(null, (string)null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (string)null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(null, (Uri)null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (Uri)null, option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        public void Given_Error_Response_With_HttpCompletionOption_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead)]
        public void Given_Validation_Error_With_HttpCompletionOption_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode, HttpCompletionOption option)
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
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead)]
        public async Task Given_Validation_Result_With_HttpCompletionOption_When_GetAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var result1 = await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path).ConfigureAwait(false))
            using (var result2 = await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path).ConfigureAwait(false))
            {
                result1.Should().Be(response);
                result2.Should().Be(response);
            }
        }

        [DataTestMethod]
        [DataRow(HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpCompletionOption.ResponseContentRead)]
        public void Given_Null_Parameters_With_HttpCompletionOption_And_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var token = default(CancellationToken);

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            {
                func = async () => await HttpClientExtensions.GetAsync(null, (string)null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (string)null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(null, (Uri)null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, (Uri)null, option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, null, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, null, token).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.BadRequest, HttpCompletionOption.ResponseContentRead)]
        public void Given_Error_Response_With_HttpCompletionOption_And_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead)]
        public void Given_Validation_Error_With_HttpCompletionOption_And_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode, HttpCompletionOption option)
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
            using (var source = this._fixture.CreateCancellationTokenSource())
            {
                func = async () => await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();

                func = async () => await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path, source.Token).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseHeadersRead)]
        [DataRow(HttpStatusCode.OK, HttpCompletionOption.ResponseContentRead)]
        public async Task Given_Validation_Result_With_HttpCompletionOption_And_CancellationToken_When_GetAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode, HttpCompletionOption option)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            using (var source = this._fixture.CreateCancellationTokenSource())
            using (var result1 = await HttpClientExtensions.GetAsync(httpClient, requestUri, option, validator.Object, path, source.Token).ConfigureAwait(false))
            using (var result2 = await HttpClientExtensions.GetAsync(httpClient, new Uri(requestUri), option, validator.Object, path, source.Token).ConfigureAwait(false))
            {
                result1.Should().Be(response);
                result2.Should().Be(response);
            }
        }

        [TestMethod]
        public void Given_Null_Parameters_When_GetStringAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();

            var func = default(Func<Task>);
            using (var httpClient = this._fixture.CreateHttpClient())
            {
                func = async () => await HttpClientExtensions.GetStringAsync(null, (string)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, (string)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, requestUri, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, requestUri, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(null, (Uri)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, (Uri)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, new Uri(requestUri), null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, new Uri(requestUri), validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        public void Given_Error_Response_When_GetStringAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var validator = new Mock<ISchemaValidator>();
            var path = "default.json";

            var func = default(Func<Task>);
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<HttpRequestException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public void Given_Validation_Error_When_GetStringAsync_Invoked_Then_It_Should_Throw_Exception(HttpStatusCode statusCode)
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
            {
                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }

            using (var handler = this._fixture.CreateFakeHttpMessageHandler(statusCode, payload))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                func = async () => await HttpClientExtensions.GetStringAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        public async Task Given_Validation_Result_When_GetStringAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode)
        {
            var requestUri = "http://localhost";
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                var result = await HttpClientExtensions.GetStringAsync(httpClient, requestUri, validator.Object, path).ConfigureAwait(false);

                result.Should().Be(payload);
            }

            using (var response = this._fixture.CreateHttpResponseMessage(statusCode, payload))
            using (var handler = this._fixture.CreateFakeHttpMessageHandler(response))
            using (var httpClient = this._fixture.CreateHttpClient(handler))
            {
                var result = await HttpClientExtensions.GetStringAsync(httpClient, new Uri(requestUri), validator.Object, path).ConfigureAwait(false);

                result.Should().Be(payload);
            }
        }
    }
}
