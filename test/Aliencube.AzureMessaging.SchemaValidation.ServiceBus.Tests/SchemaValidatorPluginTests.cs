using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

#if NETCOREAPP2_1
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus.Tests
{
#if NETCOREAPP2_1
    [TestClass]
    public class SchemaValidatorPluginTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(SchemaValidatorPlugin).Should().BeDerivedFrom<ServiceBusPlugin>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaValidatorPlugin).Should().Implement<ISchemaValidatorPlugin>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaValidatorPlugin)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(bool) })
                .And.HaveConstructor(new[] { typeof(string), typeof(bool) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaValidatorPlugin)
                .Should().HaveProperty<string>("Name")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();

            typeof(SchemaValidatorPlugin)
                .Should().HaveProperty<bool>("ShouldContinueOnException")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();

            typeof(SchemaValidatorPlugin)
                .Should().HaveProperty<ISchemaValidator>("Validator")
                    .Which.Should().BeReadable()
                          .And.BeWritable();

            typeof(SchemaValidatorPlugin)
                .Should().HaveProperty<string>("SchemaPathUserPropertyKey")
                    .Which.Should().BeReadable()
                          .And.BeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaValidatorPlugin)
                .Should().HaveMethod("WithValidator", new[] { typeof(ISchemaValidator) })
                    .Which.Should().Return<ServiceBusPlugin>();

            typeof(SchemaValidatorPlugin)
                .Should().HaveMethod("WithSchemaPathUserPropertyKey", new[] { typeof(string) })
                    .Which.Should().Return<ServiceBusPlugin>();

            typeof(SchemaValidatorPlugin)
                .Should().HaveMethod("BeforeMessageSend", new[] { typeof(Message) })
                    .Which.Should().Return<Task<Message>>();

            typeof(SchemaValidatorPlugin)
                .Should().HaveMethod("AfterMessageReceive", new[] { typeof(Message) })
                    .Which.Should().Return<Task<Message>>();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Default_Properties()
        {
            var instance = new SchemaValidatorPlugin();

            instance.Name.Should().Be(typeof(SchemaValidatorPlugin).FullName);
            instance.ShouldContinueOnException.Should().BeFalse();
            instance.Validator.Should().BeNull();
            instance.SchemaPathUserPropertyKey.Should().Be("schemaPath");
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Given_ShouldContinueOnException_When_Instantiated_Then_It_Should_Return_Value(bool shouldContinueOnException)
        {
            var instance = new SchemaValidatorPlugin(shouldContinueOnException);

            instance.Name.Should().Be(typeof(SchemaValidatorPlugin).FullName);
            instance.ShouldContinueOnException.Should().Be(shouldContinueOnException);
            instance.Validator.Should().BeNull();
            instance.SchemaPathUserPropertyKey.Should().Be("schemaPath");
        }

        [TestMethod]
        public void Given_Null_SchemaPath_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaValidatorPlugin(null, true);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("helloWorld", true)]
        [DataRow("helloWorld", false)]
        public void Given_Parameters_When_Instantiated_Then_It_Should_Return_Value(string schemaPathUserPropertyKey, bool shouldContinueOnException)
        {
            var instance = new SchemaValidatorPlugin(schemaPathUserPropertyKey, shouldContinueOnException);

            instance.Name.Should().Be(typeof(SchemaValidatorPlugin).FullName);
            instance.ShouldContinueOnException.Should().Be(shouldContinueOnException);
            instance.Validator.Should().BeNull();
            instance.SchemaPathUserPropertyKey.Should().Be(schemaPathUserPropertyKey);
        }

        [TestMethod]
        public void Given_Null_Validator_When_WithValidator_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaValidatorPlugin();

            Action action = () => instance.WithValidator(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Validator_When_WithValidator_Invoked_Then_It_Should_Return_Result()
        {
            var validator = new FakeSchemaValidator();
            var instance = new SchemaValidatorPlugin();

            var result = instance.WithValidator(validator);

            result.As<ISchemaValidatorPlugin>().Validator.Should().Be(validator);
        }

        [TestMethod]
        public void Given_Null_SchemaPathUserPropertyKey_When_WithValidator_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaValidatorPlugin();

            Action action = () => instance.WithSchemaPathUserPropertyKey(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello-world")]
        public void Given_SchemaPathUserPropertyKey_When_WithValidator_Invoked_Then_It_Should_Return_Result(string schemaPathUserPropertyKey)
        {
            var instance = new SchemaValidatorPlugin();

            var result = instance.WithSchemaPathUserPropertyKey(schemaPathUserPropertyKey);

            result.As<ISchemaValidatorPlugin>().SchemaPathUserPropertyKey.Should().Be(schemaPathUserPropertyKey);
        }

        [TestMethod]
        public void Given_Null_Message_When_BeforeMessageSend_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.BeforeMessageSend(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Empty_Message_When_BeforeMessageSend_Invoked_Then_It_Should_Throw_Exception()
        {
            var body = Array.Empty<byte>();
            var message = new Message(body);
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.BeforeMessageSend(message).ConfigureAwait(false);

            func.Should().Throw<MessageBodyZeroLengthException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_Without_SchemaPath_When_BeforeMessageSend_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.BeforeMessageSend(message).ConfigureAwait(false);

            func.Should().Throw<KeyNotFoundException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_With_Empty_SchemaPath_When_BeforeMessageSend_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", string.Empty);

            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.BeforeMessageSend(message).ConfigureAwait(false);

            func.Should().Throw<SchemaPathNotExistException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public void Given_Validation_Error_When_BeforeMessageSend_Invoked_Then_It_Should_Throw_Exception(string body, string path)
        {
            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", path);

            var instance = new SchemaValidatorPlugin()
                               .WithValidator(validator.Object);

            Func<Task> func = async () => await instance.BeforeMessageSend(message).ConfigureAwait(false);

            func.Should().Throw<SchemaValidationException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public async Task Given_Message_When_BeforeMessageSend_Invoked_Then_It_Should_Return_Result(string body, string path)
        {
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", path);

            var instance = new SchemaValidatorPlugin()
                               .WithValidator(validator.Object);

            var result = await instance.BeforeMessageSend(message).ConfigureAwait(false);

            result.Should().Be(message);
        }

        [TestMethod]
        public void Given_Null_Message_When_AfterMessageReceive_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.AfterMessageReceive(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Empty_Message_When_AfterMessageReceive_Invoked_Then_It_Should_Throw_Exception()
        {
            var body = Array.Empty<byte>();
            var message = new Message(body);
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.AfterMessageReceive(message).ConfigureAwait(false);

            func.Should().Throw<MessageBodyZeroLengthException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_Without_SchemaPath_When_AfterMessageReceive_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.AfterMessageReceive(message).ConfigureAwait(false);

            func.Should().Throw<KeyNotFoundException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_With_Empty_SchemaPath_When_AfterMessageReceive_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", string.Empty);

            var instance = new SchemaValidatorPlugin();

            Func<Task> func = async () => await instance.AfterMessageReceive(message).ConfigureAwait(false);

            func.Should().Throw<SchemaPathNotExistException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public void Given_Validation_Error_When_AfterMessageReceive_Invoked_Then_It_Should_Throw_Exception(string body, string path)
        {
            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", path);

            var instance = new SchemaValidatorPlugin()
                               .WithValidator(validator.Object);

            Func<Task> func = async () => await instance.AfterMessageReceive(message).ConfigureAwait(false);

            func.Should().Throw<SchemaValidationException>();
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public async Task Given_Message_When_AfterMessageReceive_Invoked_Then_It_Should_Return_Result(string body, string path)
        {
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var bytes = Encoding.UTF8.GetBytes(body);
            var message = new Message(bytes);
            message.UserProperties.Add("schemaPath", path);

            var instance = new SchemaValidatorPlugin()
                               .WithValidator(validator.Object);

            var result = await instance.AfterMessageReceive(message).ConfigureAwait(false);

            result.Should().Be(message);
        }
    }
#endif
}
