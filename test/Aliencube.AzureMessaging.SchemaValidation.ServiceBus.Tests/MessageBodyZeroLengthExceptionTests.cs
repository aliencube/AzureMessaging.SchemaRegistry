using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using FluentAssertions;
using FluentAssertions.Common;

#if NET461

using Microsoft.ServiceBus.Messaging;

#elif NETCOREAPP3_1
using Microsoft.Azure.ServiceBus;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus.Tests
{
    [TestClass]
    public class MessageBodyZeroLengthExceptionTests
    {
        private const string MessageBodyZeroLength = "Message body has zero length";

        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(MessageBodyZeroLengthException).Should().BeDerivedFrom<Exception>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Be_Serialisable()
        {
            typeof(MessageBodyZeroLengthException).Should().BeDecoratedWith<SerializableAttribute>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(MessageBodyZeroLengthException)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(string), typeof(Exception) })
                ;

            var constructors = typeof(MessageBodyZeroLengthException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

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
            typeof(MessageBodyZeroLengthException)
#if NET461
                .Should().HaveProperty<BrokeredMessage>("ServiceBusMessage")
#elif NETCOREAPP3_1
                .Should().HaveProperty<Message>("ServiceBusMessage")
#endif
                    .Which.Should().BeReadable()
                        .And.BeWritable()
                        ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(MessageBodyZeroLengthException)
#if NET461
                .Should().HaveMethod("WithServiceBusMessage", new[] { typeof(BrokeredMessage) })
#elif NETCOREAPP3_1
                .Should().HaveMethod("WithServiceBusMessage", new[] { typeof(Message) })
#endif
                    .Which.Should().Return<MessageBodyZeroLengthException>()
                    ;
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Message()
        {
            var ex = new MessageBodyZeroLengthException();

            ex.Message.Should().Be(MessageBodyZeroLength);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_When_Instantiated_Then_It_Should_Return_Message(string message)
        {
            var ex = new MessageBodyZeroLengthException(message);

            ex.Message.Should().Be(message);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Message_And_Exception_When_Instantiated_Then_It_Should_Return_Message_And_Exception(string message)
        {
            var innerException = new ApplicationException();
            var ex = new MessageBodyZeroLengthException(message, innerException);

            ex.Message.Should().Be(message);
            ex.InnerException.Should().Be(innerException);
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Null_Sink()
        {
            var ex = new MessageBodyZeroLengthException();

            ex.ServiceBusMessage.Should().BeNull();
        }

        [TestMethod]
        public void Given_Null_ServiceBusMessage_When_WithServiceBusMessage_Invoked_Then_It_Should_Throw_Exception()
        {
            var ex = new MessageBodyZeroLengthException();

            Action action = () => ex.WithServiceBusMessage(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_ServiceBusMessage_When_WithServiceBusMessage_Invoked_Then_It_Should_Return_Result()
        {
            var ex = new MessageBodyZeroLengthException();
#if NET461
            var message = new BrokeredMessage();
#elif NETCOREAPP3_1
            var message = new Message();
#endif
            ex.WithServiceBusMessage(message);

            ex.Should().BeOfType<MessageBodyZeroLengthException>();
#if NET461
            message.Dispose();
#endif
        }
    }
}
