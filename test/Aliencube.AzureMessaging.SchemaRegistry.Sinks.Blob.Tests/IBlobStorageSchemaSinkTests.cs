using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Tests
{
    [TestClass]
    public class IBlobStorageSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseInterface()
        {
            typeof(IBlobStorageSchemaSink).Should().Implement<ISchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(IBlobStorageSchemaSink)
                .Should().HaveProperty<string>("Container")
                    .Which.Should().BeReadable()
                        .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(IBlobStorageSchemaSink)
                .Should().HaveMethod("WithBlobClient", new[] { typeof(CloudBlobClient) })
                    .Which.Should().Return<ISchemaSink>();

            typeof(IBlobStorageSchemaSink)
                .Should().HaveMethod("WithContainer", new[] { typeof(string) })
                    .Which.Should().Return<ISchemaSink>();
        }
    }
}
