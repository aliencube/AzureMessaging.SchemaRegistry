using System.Text;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests
{
    [TestClass]
    public class IFileSystemSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseInterface()
        {
            typeof(IFileSystemSchemaSink).Should().Implement<ISchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(IFileSystemSchemaSink)
                .Should().HaveProperty<IDirectoryWrapper>("Directory")
                    .Which.Should().BeReadable()
                        .And.BeWritable();

            typeof(IFileSystemSchemaSink)
                .Should().HaveProperty<IFileWrapper>("File")
                    .Which.Should().BeReadable()
                        .And.BeWritable();

            typeof(IFileSystemSchemaSink)
                .Should().HaveProperty<Encoding>("Encoding")
                    .Which.Should().BeReadable()
                        .And.BeWritable();
        }
    }
}
