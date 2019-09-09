using System;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;

using Moq;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Tests.Extensions
{
    [TestClass]
    public class SchemaSinkExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(SchemaSinkExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name.IsEquivalentTo("WithBlobClient", StringComparison.CurrentCulture))
                .And.Contain(p => p.Name.IsEquivalentTo("WithContainer", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_WithBlobClient_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => SchemaSinkExtensions.WithBlobClient(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => SchemaSinkExtensions.WithBlobClient(new Mock<ISchemaSink>().Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Invalid_Type_When_WithBlobClient_Invoked_Then_It_Should_Throw_Exception()
        {
            var sink = new FakeSchemaSink();

            Action action = () => SchemaSinkExtensions.WithBlobClient(sink, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_HttpClient_When_WithHttpClient_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new BlobStorageSchemaSink();
            var uri = new Uri("https://localhost");
            var blobClient = new CloudBlobClient(uri);

            var result = SchemaSinkExtensions.WithBlobClient(sink, blobClient);

            result
                .Should().NotBeNull()
                .And.BeAssignableTo<ISchemaSink>();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_WithContainer_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => SchemaSinkExtensions.WithContainer(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => SchemaSinkExtensions.WithContainer(new Mock<ISchemaSink>().Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Invalid_Type_When_WithContainer_Invoked_Then_It_Should_Throw_Exception()
        {
            var sink = new FakeSchemaSink();

            Action action = () => SchemaSinkExtensions.WithContainer(sink, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_HttpClient_When_WithContainer_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new BlobStorageSchemaSink();
            var uri = new Uri("https://localhost");
            var container = "lorem-ipsum";

            var result = SchemaSinkExtensions.WithContainer(sink, container);

            result
                .Should().NotBeNull()
                .And.BeAssignableTo<ISchemaSink>();
        }
    }
}
