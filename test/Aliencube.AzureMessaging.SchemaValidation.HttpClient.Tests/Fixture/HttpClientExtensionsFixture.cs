using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;

using WorldDomination.Net.Http;

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests.Fixture
{
    [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public class HttpClientExtensionsFixture : IDisposable
    {
        private bool _disposed;

        public HttpContent CreateHttpContent(string payload = null)
        {
            var content = payload.IsNullOrWhiteSpace()
                ? new ByteArrayContent(Array.Empty<byte>())
                : new StringContent(payload);

            return content;
        }

        public HttpRequestMessage CreateHttpRequestMessage(string verb, string requestUri, string payload = null)
        {
            return this.CreateHttpRequestMessage(verb, new Uri(requestUri), payload);
        }

        public HttpRequestMessage CreateHttpRequestMessage(string verb, Uri requestUri, string payload = null)
        {
            var method = new HttpMethod(verb);
            var request = new HttpRequestMessage(method, requestUri);
            if (!payload.IsNullOrWhiteSpace())
            {
                var content = this.CreateHttpContent(payload);
                request.Content = content;
            }

            return request;
        }

        public HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, string payload = null)
        {
            var response = new HttpResponseMessage(statusCode);
            if (!payload.IsNullOrWhiteSpace())
            {
                var content = this.CreateHttpContent(payload);
                response.Content = content;
            }

            return response;
        }

        public FakeHttpMessageHandler CreateFakeHttpMessageHandler(HttpStatusCode statusCode, string payload = null)
        {
            var response = this.CreateHttpResponseMessage(statusCode, payload);
            var handler = this.CreateFakeHttpMessageHandler(response);

            return handler;
        }

        public FakeHttpMessageHandler CreateFakeHttpMessageHandler(HttpResponseMessage response)
        {
            var options = new HttpMessageOptions() { HttpResponseMessage = response };
            var handler = new FakeHttpMessageHandler(options);

            return handler;
        }

        public System.Net.Http.HttpClient CreateHttpClient(HttpMessageHandler handler = null)
        {
            if (handler.IsNullOrDefault())
            {
                return new System.Net.Http.HttpClient();
            }

            return new System.Net.Http.HttpClient(handler);
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects).
            // TODO: set large fields to null.

            this._disposed = true;
        }
    }
}
