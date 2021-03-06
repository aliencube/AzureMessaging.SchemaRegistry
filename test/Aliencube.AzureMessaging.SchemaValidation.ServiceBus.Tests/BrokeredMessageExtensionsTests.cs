using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

#if NET461

using Microsoft.ServiceBus.Messaging;

#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaValidation.ServiceBus.Tests
{
#if NET461

    [TestClass]
    public class BrokeredMessageExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(BrokeredMessageExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name == "ValidateAsync")
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var validator = new Mock<ISchemaValidator>();
            using (var message = new BrokeredMessage())
            {
                var func = default(Func<Task>);

                func = async () => await BrokeredMessageExtensions.ValidateAsync((BrokeredMessage)null, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), null, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();

                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object, null).ConfigureAwait(false);
                func.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public void Given_Null_Body_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            using (var stream = new MemoryStream(Array.Empty<byte>()))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);
                func.Should().Throw<MessageBodyZeroLengthException>();
            }

            using (var stream = new MemoryStream(Array.Empty<byte>()))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);
                func.Should().Throw<MessageBodyZeroLengthException>();
            }
        }

        [TestMethod]
        public void Given_Empty_Body_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty)))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);
                func.Should().Throw<MessageBodyZeroLengthException>();
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty)))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);
                func.Should().Throw<MessageBodyZeroLengthException>();
            }
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_Without_SchemaPath_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);
                func.Should().Throw<KeyNotFoundException>();
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);
                func.Should().Throw<KeyNotFoundException>();
            }
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }")]
        public void Given_Message_With_Empty_SchemaPath_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string body)
        {
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", string.Empty);

                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);
                func.Should().Throw<SchemaPathNotExistException>();
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", string.Empty);

                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);
                func.Should().Throw<SchemaPathNotExistException>();
            }
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public void Given_Validation_Error_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception(string body, string path)
        {
            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var func = default(Func<Task>);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", path);

                func = async () => await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", path);

                func = async () => await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);
                func.Should().Throw<SchemaValidationException>();
            }
        }

        [DataTestMethod]
        [DataRow("{ \"hello\": \"world\" }", "default.json")]
        public async Task Given_Message_When_BeforeMessageSend_Invoked_Then_It_Should_Return_Result(string body, string path)
        {
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", path);

                var result = await BrokeredMessageExtensions.ValidateAsync(message, validator.Object).ConfigureAwait(false);

                result.Should().Be(message);
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            using (var message = new BrokeredMessage(stream))
            {
                message.Properties.Add("schemaPath", path);

                var result = await BrokeredMessageExtensions.ValidateAsync(Task.FromResult(message), validator.Object).ConfigureAwait(false);

                result.Should().Be(message);
            }
        }
    }

#endif
}
