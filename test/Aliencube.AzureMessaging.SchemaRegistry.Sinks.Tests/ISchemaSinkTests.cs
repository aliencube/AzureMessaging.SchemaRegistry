using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Tests
{
    [TestClass]
    public class ISchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(ISchemaSink)
                .Should().HaveProperty<string>("Name")
                    .Which.Should().BeReadable()
                        .And.BeWritable();

            typeof(ISchemaSink)
                .Should().HaveProperty<string>("BaseLocation")
                    .Which.Should().BeReadable()
                        .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(ISchemaSink)
                .Should().HaveMethod("WithBaseLocation", new[] { typeof(string) })
                    .Which.Should().Return<ISchemaSink>();

            typeof(ISchemaSink)
                .Should().HaveMethod("GetSchemaAsync", new[] { typeof(string) })
                    .Which.Should().Return<Task<string>>();

            typeof(ISchemaSink)
                .Should().HaveMethod("SetSchemaAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().Return<Task<bool>>()
                    ;
        }
    }
}
