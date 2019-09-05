using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests.Sinks
{
    [TestClass]
    [SuppressMessage("Usage", "CA1806:Do not ignore method results")]
    public class SchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Be_Abstract()
        {
            typeof(SchemaSink).Should().BeAbstract();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaSink).Should().Implement<ISchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaSink)
                .Should().HaveDefaultConstructor();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaSink)
                .Should().HaveProperty<string>("BaseLocation")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaSink)
                .Should().HaveMethod("WithBaseLocation", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaSink>();

            typeof(SchemaSink)
                .Should().HaveMethod("GetSchemaAsync", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<string>>();

            typeof(SchemaSink)
                .Should().HaveMethod("SetSchemaAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<bool>>();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Value()
        {
            var instance = new FakeSchemaSink();

            instance.BaseLocation.Should().BeEmpty();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new FakeSchemaSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Location_When_Instantiated_Then_It_Should_Return_Value(string location)
        {
            var instance = new FakeSchemaSink(location);

            instance.BaseLocation.Should().Be(location);
        }

        [TestMethod]
        public void Given_Null_Location_When_WithBaseLocation_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new FakeSchemaSink();

            Action action = () => instance.WithBaseLocation(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(@"c:\\")]
        public void Given_Location_When_WithBaseLocation_Invoked_Then_It_Should_Return_Result(string location)
        {
            var instance = new FakeSchemaSink();

            var result = instance.WithBaseLocation(location);

            result.BaseLocation.Should().Be(location);
        }

        [DataTestMethod]
        [DataRow(@"c:\\", "default.json")]
        public async Task Given_Location_When_GetSchemaAsync_Invoked_Then_It_Should_Return_Result(string location, string path)
        {
            var instance = new FakeSchemaSink(location);

            var result = await instance.GetSchemaAsync(path).ConfigureAwait(false);

            result.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(@"c:\\", "default.json", "{ \"hello\": \"world\" }")]
        public async Task Given_Location_And_Schema_When_SetSchemaAsync_Invoked_Then_It_Should_Return_Result(string location, string path, string schema)
        {
            var instance = new FakeSchemaSink(location);

            var result = await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            result.Should().BeTrue();
        }
    }
}
