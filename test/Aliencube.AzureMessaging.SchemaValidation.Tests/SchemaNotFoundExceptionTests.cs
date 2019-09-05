using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    public class SchemaNotFoundExceptionTests
    {
        private const string SchemaNotFound = "Schema not found";

        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(SchemaNotFoundException).Should().BeDerivedFrom<Exception>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Be_Serialisable()
        {
            typeof(SchemaNotFoundException).Should().BeDecoratedWith<SerializableAttribute>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaNotFoundException)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(string), typeof(Exception) })
                ;

            var constructors = typeof(SchemaNotFoundException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

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
            typeof(SchemaNotFoundException)
                .Should().HaveProperty<ISchemaSink>("Sink")
                    .Which.Should().BeReadable()
                        .And.BeWritable()
                        ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaNotFoundException)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<SchemaNotFoundException>()
                    ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Message()
        {
            var ex = new SchemaNotFoundException();

            ex.Message.Should().Be(SchemaNotFound);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_When_Instantiated_Then_It_Should_Return_Message(string message)
        {
            var ex = new SchemaNotFoundException(message);

            ex.Message.Should().Be(message);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_And_Exception_When_Instantiated_Then_It_Should_Return_Message_And_Exception(string message)
        {
            var innerException = new ApplicationException();
            var ex = new SchemaNotFoundException(message, innerException);

            ex.Message.Should().Be(message);
            ex.InnerException.Should().Be(innerException);
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Sink()
        {
            var ex = new SchemaNotFoundException();

            ex.Sink.Should().BeNull();
        }

        [TestMethod]
        public void Given_Null_Sink_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            var ex = new SchemaNotFoundException();

            Action action = () => ex.WithSink(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Result()
        {
            var sink = new FakeSchemaSink();

            var ex = new SchemaNotFoundException()
                         .WithSink(sink);

            ex.Should().BeOfType<SchemaNotFoundException>();
        }
    }
}
