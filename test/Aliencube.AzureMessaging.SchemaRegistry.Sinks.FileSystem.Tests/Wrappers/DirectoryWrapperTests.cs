using System;
using System.IO;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests.Wrappers
{
    [TestClass]
    public class DirectoryWrapperTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(DirectoryWrapper)
                .Should().HaveDefaultConstructor();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(DirectoryWrapper)
                .Should().HaveMethod("CreateDirectory", new[] { typeof(string) })
                    .Which.Should().Return<DirectoryInfo>();
        }

        [TestMethod]
        public void Given_Null_Path_When_CreateDirectory_Invoked_Then_It_Should_Throw_Exception()
        {
            var wrapper = new DirectoryWrapper();

            Action action = () => wrapper.CreateDirectory(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello-world", true)]
        public void Given_Path_When_CreateDirectory_Invoked_Then_It_Should_Return_Result(string directory, bool expected)
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(baseDirectory, directory);

            var wrapper = new DirectoryWrapper();

            var result = wrapper.CreateDirectory(path);

            result.Exists.Should().Be(expected);
        }
    }
}
