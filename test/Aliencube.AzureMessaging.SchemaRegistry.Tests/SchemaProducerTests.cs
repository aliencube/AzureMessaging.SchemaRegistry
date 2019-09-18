using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NJsonSchema;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class SchemaProducerTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaProducer).Should().Implement<ISchemaProducer>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaProducer)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(ISchemaBuilder) })
                .And.HaveConstructor(new[] { typeof(List<ISchemaSink>) })
                .And.HaveConstructor(new[] { typeof(ISchemaBuilder), typeof(List<ISchemaSink>) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaProducer)
                .Should().HaveProperty<ISchemaBuilder>("Builder")
                    .Which.Should().BeReadable()
                          .And.BeWritable();

            typeof(SchemaProducer)
                .Should().HaveProperty<List<ISchemaSink>>("Sinks")
                    .Which.Should().BeReadable()
                          .And.BeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaProducer)
                .Should().HaveMethod("WithBuilder", new[] { typeof(ISchemaBuilder) })
                    .Which.Should().Return<ISchemaProducer>();

            typeof(SchemaProducer)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<ISchemaProducer>();

            typeof(SchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeAsync()
                    .And.Return<Task<bool>>()
                    ;

            typeof(SchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(Type), typeof(string) })
                    .Which.Should().BeAsync()
                    .And.Return<Task<bool>>()
                    ;

            typeof(SchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(string) })
                    .Which.Should().BeAsync()
                    .And.Return<Task<bool>>()
                    ;

            typeof(SchemaProducer).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => p.IsGenericMethodDefinition)
                                   .SingleOrDefault(p => p.Name.IsEquivalentTo("ProduceAsync", StringComparison.CurrentCulture))
                .Should().NotBeNull()
                .And.BeAsync()
                .And.Return<Task<bool>>()
                .And.Subject.GetParameters()
                    .Should().HaveCount(1)
                ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_NullOrEmpty_Results()
        {
            var instance = new SchemaProducer();

            instance.Builder.Should().BeNull();

            instance.Sinks.Should().NotBeNull()
                          .And.HaveCount(0);
        }

        [TestMethod]
        public void Given_Null_Builder_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(builder: null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_Builder_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(default(ISchemaBuilder));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_Sinks_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(sinks: null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_Sinks_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(default(List<ISchemaSink>));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new SchemaProducer(new Mock<ISchemaBuilder>().Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer(default(ISchemaBuilder), null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new SchemaProducer(new Mock<ISchemaBuilder>().Object, default(List<ISchemaSink>));
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Builder_When_Instantiated_Then_It_Should_Return_Values()
        {
            var builder = new Mock<ISchemaBuilder>();
            var instance = new SchemaProducer(builder.Object);

            instance.Builder.Should().NotBeNull();

            instance.Sinks.Should().NotBeNull()
                          .And.HaveCount(0);
        }

        [TestMethod]
        public void Given_Sinks_When_Instantiated_Then_It_Should_Return_Values()
        {
            var sinks = new List<ISchemaSink>();
            var instance = new SchemaProducer(sinks);

            instance.Builder.Should().BeNull();

            instance.Sinks.Should().NotBeNull()
                          .And.HaveCount(0);
        }

        [TestMethod]
        public void Given_Null_When_WithBuilder_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer()
                                      .WithBuilder(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_When_WithBuilder_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer()
                                      .WithBuilder(default(ISchemaBuilder));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Builder_When_WithBuilder_Invoked_Then_It_Should_Return_Values()
        {
            var builder = new Mock<ISchemaBuilder>();
            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object);

            instance.Builder
                .Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Null_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer()
                                      .WithSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaProducer()
                                      .WithSink(default(ISchemaSink));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Values()
        {
            var sink = new Mock<ISchemaSink>();
            var instance = new SchemaProducer()
                               .WithSink(sink.Object);

            instance.Sinks
                .Should().NotBeNull()
                .And.HaveCount(1);
        }

        [TestMethod]
        public void Given_Null_Schema_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var schema = "{ \"hello\": \"world\" }";

            var instance = new SchemaProducer();

            Func<Task> func = async () => await instance.ProduceAsync(schema: null, path: null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.ProduceAsync(schema, path: null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Schema_And_Empty_Sink_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var schema = "{ \"hello\": \"world\" }";
            var path = "default.json";
            var instance = new SchemaProducer();

            Func<Task> func = async () => await instance.ProduceAsync(schema, path).ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Given_Schema_And_Error_Sink_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var schema = "{ \"hello\": \"world\" }";
            var path = "default.json";

            var sink1 = new Mock<ISchemaSink>();
            sink1.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            var sink2 = new Mock<ISchemaSink>();
            sink2.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var instance = new SchemaProducer()
                               .WithSink(sink1.Object)
                               .WithSink(sink2.Object);

            Func<Task> func = async () => await instance.ProduceAsync(schema, path).ConfigureAwait(false);

            func.Should().Throw<AggregateException>();
        }

        [TestMethod]
        public async Task Given_Schema_And_Sinks_When_ProduceAsync_Invoked_Then_It_Should_Return_Result()
        {
            var schema = "{ \"hello\": \"world\" }";
            var path = "default.json";

            var sink1 = new Mock<ISchemaSink>();
            sink1.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var sink2 = new Mock<ISchemaSink>();
            sink2.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var instance = new SchemaProducer()
                               .WithSink(sink1.Object)
                               .WithSink(sink2.Object);

            var result = await instance.ProduceAsync(schema, path).ConfigureAwait(false);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_Null_Type_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object);

            Func<Task> func = async () => await instance.ProduceAsync(type: null, path: null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.ProduceAsync(typeof(FakeClass), path: null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Type_And_Empty_Sink_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var path = "default.json";

            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object);

            Func<Task> func = async () => await instance.ProduceAsync(typeof(FakeClass), path).ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Given_Type_And_Error_Sink_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var path = "default.json";

            var type = typeof(FakeClass);
            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var sink1 = new Mock<ISchemaSink>();
            sink1.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            var sink2 = new Mock<ISchemaSink>();
            sink2.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object)
                               .WithSink(sink1.Object)
                               .WithSink(sink2.Object);

            Func<Task> func = async () => await instance.ProduceAsync(type, path).ConfigureAwait(false);

            func.Should().Throw<AggregateException>();
        }

        [TestMethod]
        public async Task Given_Type_And_Sinks_When_ProduceAsync_Invoked_Then_It_Should_Return_Result()
        {
            var path = "default.json";

            var type = typeof(FakeClass);
            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var sink1 = new Mock<ISchemaSink>();
            sink1.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var sink2 = new Mock<ISchemaSink>();
            sink2.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object)
                               .WithSink(sink1.Object)
                               .WithSink(sink2.Object);

            var result = await instance.ProduceAsync(type, path).ConfigureAwait(false);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_TypeParameter_And_Empty_Sink_When_ProduceAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var path = "default.json";

            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object);

            Func<Task> func = async () => await instance.ProduceAsync<FakeClass>(path).ConfigureAwait(false);

            func.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public async Task Given_TypeParameter_And_Sinks_When_ProduceAsync_Invoked_Then_It_Should_Return_Result()
        {
            var path = "default.json";

            var schema = new JsonSchema();

            var builder = new Mock<ISchemaBuilder>();
            builder.Setup(p => p.Build(It.IsAny<Type>())).Returns(schema);

            var sink1 = new Mock<ISchemaSink>();
            sink1.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var sink2 = new Mock<ISchemaSink>();
            sink2.Setup(p => p.SetSchemaAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var instance = new SchemaProducer()
                               .WithBuilder(builder.Object)
                               .WithSink(sink1.Object)
                               .WithSink(sink2.Object);

            var result = await instance.ProduceAsync<FakeClass>(path).ConfigureAwait(false);

            result.Should().BeTrue();
        }
    }
}
