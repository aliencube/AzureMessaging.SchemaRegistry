using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests
{
    [TestClass]
    public class FileSystemSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(FileSystemSchemaSink).Should().BeDerivedFrom<SchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(FileSystemSchemaSink)
                .Should().Implement<ISchemaSink>()
                .And.Implement<IFileSystemSchemaSink>()
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(FileSystemSchemaSink)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) });
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(FileSystemSchemaSink)
                .Should().HaveProperty<IDirectoryWrapper>("Directory")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(FileSystemSchemaSink)
                .Should().HaveProperty<IFileWrapper>("File")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;

            typeof(FileSystemSchemaSink)
                .Should().HaveProperty<Encoding>("Encoding")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(FileSystemSchemaSink)
                .Should().HaveMethod("GetSchemaAsync", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<string>>();

            typeof(FileSystemSchemaSink)
                .Should().HaveMethod("SetSchemaAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<bool>>();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Value()
        {
            var instance = new FileSystemSchemaSink();

            instance.BaseLocation.Should().BeEmpty();
            instance.Directory.Should().NotBeNull();
            instance.File.Should().NotBeNull();
            instance.Encoding.Should().Be(Encoding.UTF8);
        }

        [TestMethod]
        public void Given_DirectoryWrapper_When_Instantiated_Then_It_Should_Return_Value()
        {
            var wrapper = new FakeDirectoryWrapper();

            var instance = new FileSystemSchemaSink() { Directory = wrapper };

            instance.BaseLocation.Should().BeEmpty();
            instance.Directory.Should().Be(wrapper);
            instance.File.Should().NotBeNull();
            instance.Encoding.Should().Be(Encoding.UTF8);
        }

        [TestMethod]
        public void Given_FileWrapper_When_Instantiated_Then_It_Should_Return_Value()
        {
            var wrapper = new FakeFileWrapper();

            var instance = new FileSystemSchemaSink() { File = wrapper };

            instance.BaseLocation.Should().BeEmpty();
            instance.Directory.Should().NotBeNull();
            instance.File.Should().Be(wrapper);
            instance.Encoding.Should().Be(Encoding.UTF8);
        }

        [TestMethod]
        public void Given_Encoding_When_Instantiated_Then_It_Should_Return_Value()
        {
            var encoding = Encoding.ASCII;

            var instance = new FileSystemSchemaSink() { Encoding = encoding };

            instance.BaseLocation.Should().BeEmpty();
            instance.Directory.Should().NotBeNull();
            instance.File.Should().NotBeNull();
            instance.Encoding.Should().Be(encoding);
        }

        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new FileSystemSchemaSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Location_When_Instantiated_Then_It_Should_Return_Value(string location)
        {
            var instance = new FileSystemSchemaSink(location);

            instance.BaseLocation.Should().Be(location);
            instance.Directory.Should().NotBeNull();
            instance.File.Should().NotBeNull();
            instance.Encoding.Should().Be(Encoding.UTF8);
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Encoding_And_Location_When_Instantiated_Then_It_Should_Return_Value(string location)
        {
            var encoding = Encoding.ASCII;
            var instance = new FileSystemSchemaSink(location) { Encoding = encoding };

            instance.BaseLocation.Should().Be(location);
            instance.Directory.Should().NotBeNull();
            instance.File.Should().NotBeNull();
            instance.Encoding.Should().Be(encoding);
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Null_Path_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string location)
        {
            var instance = new FileSystemSchemaSink(location);

            Func<Task> func = async () => await instance.GetSchemaAsync(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(@"c:\\", "default.json")]
        public void Given_Invalid_Path_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string location, string path)
        {
            var file = new Mock<IFileWrapper>();
            file.Setup(p => p.Exists(It.IsAny<string>())).Returns(false);

            var instance = new FileSystemSchemaSink(location) { File = file.Object };

            Func<Task> func = async () => await instance.GetSchemaAsync(path).ConfigureAwait(false);

            func.Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        public async Task Given_Location_When_GetSchemaAsync_Invoked_Then_It_Should_Return_Result()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = "default.json";

            var instance = new FileSystemSchemaSink(location);

            var result = await instance.GetSchemaAsync(path).ConfigureAwait(false);

            result.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Null_Path_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string location)
        {
            var schema = "{ \"hello\": \"world\" }";

            var instance = new FileSystemSchemaSink(location);

            Func<Task> func = async () => await instance.SetSchemaAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.SetSchemaAsync(schema, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(@"c:\\", "default.json", "{ \"hello\": \"world\" }")]
        public async Task Given_Location_And_Path_And_Schema_When_SetSchemaAsync_Invoked_Then_It_Should_Return_Result(string location, string path, string schema)
        {
            var instance = new FileSystemSchemaSink(location);

            var result = await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            result.Should().BeTrue();
        }
    }
}
