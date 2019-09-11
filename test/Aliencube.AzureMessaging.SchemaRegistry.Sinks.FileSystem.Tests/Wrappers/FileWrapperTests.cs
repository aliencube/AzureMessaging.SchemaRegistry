using System.IO;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests.Wrappers
{
    [TestClass]
    public class FileWrapperTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(FileWrapper)
                .Should().HaveDefaultConstructor();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(FileWrapper)
                .Should().HaveMethod("Exists", new[] { typeof(string) })
                    .Which.Should().Return<bool>();
        }

        [TestMethod]
        public void Given_Null_Path_When_Exists_Invoked_Then_It_Should_Return_False()
        {
            var wrapper = new FileWrapper();

            var result = wrapper.Exists(null);

            result.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("helloworld.txt", false)]
        [DataRow("default.json", true)]
        public void Given_Path_When_Exists_Invoked_Then_It_Should_Return_Result(string filename, bool expected)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(directory, filename);

            var wrapper = new FileWrapper();

            var result = wrapper.Exists(path);

            result.Should().Be(expected);
        }
    }
}
