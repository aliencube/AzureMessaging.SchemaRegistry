using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class ISchemaConsumerTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(ISchemaConsumer)
                .Should().HaveProperty<ISchemaSink>("Sink")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(ISchemaConsumer)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<ISchemaConsumer>();

            typeof(ISchemaConsumer)
                .Should().HaveMethod("ConsumeAsync", new[] { typeof(string) })
                    .Which.Should().Return<Task<string>>()
                    ;
        }
    }
}
