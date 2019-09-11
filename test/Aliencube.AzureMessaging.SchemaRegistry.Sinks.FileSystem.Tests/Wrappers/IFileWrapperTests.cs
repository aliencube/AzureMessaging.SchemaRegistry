using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests.Wrappers
{
    [TestClass]
    public class IFileWrapperTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(IFileWrapper)
                .Should().HaveMethod("Exists", new[] { typeof(string) })
                    .Which.Should().Return<bool>();
        }
    }
}
