using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Aliencube.AzureMessaging.SchemaRegistry;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    public class SchemaMalformedExceptionTests
    {
        private const string SchemaNotFormed = "Schema is not formed properly";

        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(SchemaMalformedException).Should().BeDerivedFrom<Exception>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Be_Serialisable()
        {
            typeof(SchemaMalformedException).Should().BeDecoratedWith<SerializableAttribute>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaMalformedException)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(Exception) })
                .And.HaveConstructor(new[] { typeof(string), typeof(Exception) })
                ;

            var constructors = typeof(SchemaMalformedException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

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
            typeof(SchemaMalformedException)
                .Should().HaveProperty<ISchemaConsumer>("Consumer")
                    .Which.Should().BeReadable()
                        .And.BeWritable()
                        ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaMalformedException)
                .Should().HaveMethod("WithSchemaConsumer", new[] { typeof(ISchemaConsumer) })
                    .Which.Should().Return<SchemaMalformedException>()
                    ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Message()
        {
            var ex = new SchemaMalformedException();

            ex.Message.Should().Be(SchemaNotFormed);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_When_Instantiated_Then_It_Should_Return_Message(string message)
        {
            var ex = new SchemaMalformedException(message);

            ex.Message.Should().Be(message);
        }

        [TestMethod]
        public void Given_Exception_When_Instantiated_Then_It_Should_Return_Exception()
        {
            var innerException = new ApplicationException();
            var ex = new SchemaMalformedException(innerException);

            ex.Message.Should().Be(SchemaNotFormed);
            ex.InnerException.Should().Be(innerException);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_And_Exception_When_Instantiated_Then_It_Should_Return_Message_And_Exception(string message)
        {
            var innerException = new ApplicationException();
            var ex = new SchemaMalformedException(message, innerException);

            ex.Message.Should().Be(message);
            ex.InnerException.Should().Be(innerException);
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Sink()
        {
            var ex = new SchemaMalformedException();

            ex.Consumer.Should().BeNull();
        }

        [TestMethod]
        public void Given_Null_Sink_When_WithSink_Invoked_Then_It_Should_Throw_Exception()
        {
            var ex = new SchemaMalformedException();

            Action action = () => ex.WithSchemaConsumer(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Sink_When_WithSink_Invoked_Then_It_Should_Return_Result()
        {
            var consumer = new Mock<ISchemaConsumer>();

            var ex = new SchemaMalformedException()
                         .WithSchemaConsumer(consumer.Object);

            ex.Should().BeOfType<SchemaMalformedException>();
        }
    }
}
