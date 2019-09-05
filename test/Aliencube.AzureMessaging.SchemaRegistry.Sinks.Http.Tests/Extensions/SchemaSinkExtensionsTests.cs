using System;
using System.Net.Http;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Extensions;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Tests.Extensions
{
    [TestClass]
    public class SchemaSinkExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(SchemaSinkExtensions).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name.IsEquivalentTo("WithHttpClient", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_WithHttpClient_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => SchemaSinkExtensions.WithHttpClient(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => SchemaSinkExtensions.WithHttpClient(new Mock<ISchemaSink>().Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Invalid_Type_When_WithHttpClient_Invoked_Then_It_Should_Throw_Exception()
        {
            var sink = new FakeSchemaSink();

            Action action = () => SchemaSinkExtensions.WithHttpClient(sink, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_HttpClient_When_WithHttpClient_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new HttpSchemaSink();
            var httpClient = new HttpClient();

            var result = SchemaSinkExtensions.WithHttpClient(sink, httpClient);

            result
                .Should().NotBeNull()
                .And.BeAssignableTo<ISchemaSink>();

            httpClient.Dispose();
        }
    }
}
