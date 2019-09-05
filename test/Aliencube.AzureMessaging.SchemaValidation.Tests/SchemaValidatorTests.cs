using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NJsonSchema.Validation;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    [SuppressMessage("Usage", "CA1806:Do not ignore method results")]
    public class SchemaValidatorTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaValidator).Should().Implement<ISchemaValidator>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaValidator)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(ISchemaSink) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaValidator)
                .Should().HaveProperty<ISchemaSink>("Sink")
                    .Which.Should().BeVirtual()
                          .And.BeReadable()
                          .And.BeWritable()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaValidator)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaValidator>()
                        ;

            typeof(SchemaValidator)
                .Should().HaveMethod("ValidateAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.Return<Task<bool>>()
                        ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Properties()
        {
            var instance = new SchemaValidator();

            instance.Sink.Should().BeNull();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Fields()
        {
            var instance = new SchemaValidator();

            var field = typeof(SchemaValidator).GetField("_validator", BindingFlags.NonPublic | BindingFlags.Instance);

            field.FieldType.Should().Be(typeof(JsonSchemaValidator));
            field.GetValue(instance).Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Null_Sink_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaValidator(sink: null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_Sink_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaValidator();

            Action action = () => instance.WithSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new FakeSchemaSink();
            var instance = new SchemaValidator()
                               .WithSink(sink);

            var result = instance.WithSink(sink);

            result.Sink.Should().Be(sink);
        }

        [TestMethod]
        public void Given_Null_Parameters_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var payload = "{ \"hello\": \"world\" }";
            var sink = new Mock<ISchemaSink>();
            var instance = new SchemaValidator()
                               .WithSink(sink.Object);

            Func<Task> func = async () => await instance.ValidateAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.ValidateAsync(payload, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public void Given_Null_Schema_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string payload, string path)
        {
            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).ReturnsAsync((string)null);

            var instance = new SchemaValidator()
                               .WithSink(sink.Object);

            Func<Task> func = async () => await instance.ValidateAsync(payload, path).ConfigureAwait(false);

            func.Should().Throw<SchemaNotFoundException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "lorem-ipsum", "default.json")]
        public void Given_Error_Schema_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string payload, string schema, string path)
        {
            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).ReturnsAsync(schema);

            var instance = new SchemaValidator()
                               .WithSink(sink.Object);

            Func<Task> func = async () => await instance.ValidateAsync(payload, path).ConfigureAwait(false);

            func.Should().Throw<SchemaMalformedException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "{ \"type\": \"string\" }", "default.json")]
        public void Given_Validation_Error_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string payload, string schema, string path)
        {
            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).ReturnsAsync(schema);

            var instance = new SchemaValidator()
                               .WithSink(sink.Object);

            Func<Task> func = async () => await instance.ValidateAsync(payload, path).ConfigureAwait(false);

            func.Should().Throw<SchemaValidationException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "{ \"type\": \"object\", \"properties\": { \"hello\": { \"type\": \"string\" } } }", "default.json")]
        public async Task Given_Payload_When_ValidateAsync_Invoked_Then_It_Should_Return_Result(string payload, string schema, string path)
        {
            var sink = new Mock<ISchemaSink>();
            sink.Setup(p => p.GetSchemaAsync(It.IsAny<string>())).ReturnsAsync(schema);

            var instance = new SchemaValidator()
                               .WithSink(sink.Object);

            var result = await instance.ValidateAsync(payload, path).ConfigureAwait(false);

            result.Should().BeTrue();
        }
    }
}
