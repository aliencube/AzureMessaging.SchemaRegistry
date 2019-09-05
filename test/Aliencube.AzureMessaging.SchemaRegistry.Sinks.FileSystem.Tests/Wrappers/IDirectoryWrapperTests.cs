using System.IO;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests.Wrappers
{
    [TestClass]
    public class IDirectoryWrapperTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(IDirectoryWrapper)
                .Should().HaveMethod("CreateDirectory", new[] { typeof(string) })
                    .Which.Should().Return<DirectoryInfo>();
        }
    }
}
