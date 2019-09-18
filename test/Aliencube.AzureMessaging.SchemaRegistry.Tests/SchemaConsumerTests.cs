using System;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class SchemaConsumerTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaConsumer).Should().Implement<ISchemaConsumer>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaConsumer)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(ISchemaSink) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaConsumer)
                .Should().HaveProperty<ISchemaSink>("Sink")
                    .Which.Should().BeReadable()
                          .And.BeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaConsumer)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<ISchemaConsumer>();
            ;

            typeof(SchemaConsumer)
                .Should().HaveMethod("ConsumeAsync", new[] { typeof(string) })
                    .Which.Should().BeAsync()
                    .And.Return<Task<string>>()
                    ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Results()
        {
            var instance = new SchemaConsumer();

            instance.Sink.Should().BeNull();
        }

        [TestMethod]
        public void Given_Null_Sink_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaConsumer(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_Sink_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaConsumer(default(ISchemaSink));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_Instantiated_Then_It_Should_Return_Values()
        {
            var sink = new Mock<ISchemaSink>();
            var instance = new SchemaConsumer(sink.Object);

            instance.Sink.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Null_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaConsumer()
                                      .WithSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaConsumer()
                                      .WithSink(default(ISchemaSink));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Values()
        {
            var sink = new Mock<ISchemaSink>();
            var instance = new SchemaConsumer()
                               .WithSink(sink.Object);

            instance.Sink
                .Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Null_Path_When_ConsumeAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaConsumer();

            Func<Task> func = async () => await instance.ConsumeAsync(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_Sink_When_ConsumeAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var path = "default.json";

            var instance = new SchemaConsumer();

            Func<Task> func = async () => await instance.ConsumeAsync(path).ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Given_Sink_When_ConsumeAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var path = "default.json";

            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).Throws<InvalidOperationException>();

            var instance = new SchemaConsumer()
                               .WithSink(sink.Object);

            Func<Task> func = async () => await instance.ConsumeAsync(path).ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task Given_Sink_When_ConsumeAsync_Invoked_Then_It_Should_Return_Result()
        {
            var schema = "{ \"hello\": \"world\" }";
            var path = "default.json";

            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).ReturnsAsync(schema);

            var instance = new SchemaConsumer()
                               .WithSink(sink.Object);

            var result = await instance.ConsumeAsync(path).ConfigureAwait(false);

            result.Should().Be(schema);
        }
    }
}
