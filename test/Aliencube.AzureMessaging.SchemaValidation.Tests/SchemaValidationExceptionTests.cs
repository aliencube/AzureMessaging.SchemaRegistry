using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

using NJsonSchema;
using NJsonSchema.Validation;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    public class SchemaValidationExceptionTests
    {
        private const string InvalidDataAgainstSchema = "Invalid data against schema";

        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(SchemaValidationException).Should().BeDerivedFrom<Exception>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Be_Serialisable()
        {
            typeof(SchemaValidationException).Should().BeDecoratedWith<SerializableAttribute>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaValidationException)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(string), typeof(Exception) })
                ;

            var constructors = typeof(SchemaValidationException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            constructors.Should().HaveCount(1);
            constructors.First().Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            var parameters = constructors.First().GetParameters();
            parameters.Should().HaveCount(2);
            parameters.Select(p => p.ParameterType)
                .Should().Contain(typeof(SerializationInfo))
                .And.Contain(typeof(StreamingContext))
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaValidationException)
                .Should().HaveProperty<ISchemaSink>("Sink")
                    .Which.Should().BeReadable()
                        .And.BeWritable()
                        ;

            typeof(SchemaValidationException)
                .Should().HaveProperty<List<ValidationError>>("ValidationErrors")
                    .Which.Should().BeReadable()
                        .And.BeWritable()
                        ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaValidationException)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<SchemaValidationException>()
                    ;

            typeof(SchemaValidationException)
                .Should().HaveMethod("WithValidationErrors", new[] { typeof(List<ValidationError>) })
                    .Which.Should().Return<SchemaValidationException>()
                    ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Message()
        {
            var ex = new SchemaValidationException();

            ex.Message.Should().Be(InvalidDataAgainstSchema);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_When_Instantiated_Then_It_Should_Return_Message(string message)
        {
            var ex = new SchemaValidationException(message);

            ex.Message.Should().Be(message);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_And_Exception_When_Instantiated_Then_It_Should_Return_Message_And_Exception(string message)
        {
            var innerException = new ApplicationException();
            var ex = new SchemaValidationException(message, innerException);

            ex.Message.Should().Be(message);
            ex.InnerException.Should().Be(innerException);
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Sink()
        {
            var ex = new SchemaValidationException();

            ex.Sink.Should().BeNull();
        }

        [TestMethod]
        public void Given_Null_Sink_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            var ex = new SchemaValidationException();

            Action action = () => ex.WithSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new FakeSchemaSink();

            var ex = new SchemaValidationException()
                         .WithSink(sink);

            ex.Should().BeOfType<SchemaValidationException>();
        }

        [TestMethod]
        public void Given_Null_ValidationErrors_When_WithValidationErrors_Invoked_Then_It_Should_Throw_Exception()
        {
            var ex = new SchemaValidationException();

            Action action = () => ex.WithValidationErrors(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_ValidationErrors_When_WithValidationErrors_Invoked_Then_It_Should_Return_Result()
        {
            var error = new ValidationError(ValidationErrorKind.Unknown, string.Empty, string.Empty, new JObject(), new JsonSchema());
            var errors = new[] { error }.ToList();

            var ex = new SchemaValidationException()
                         .WithValidationErrors(errors);

            ex.Should().BeOfType<SchemaValidationException>();
        }
    }
}
