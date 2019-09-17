using System;
using System.Reflection;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(StringExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name == "ValidateAsync")
                .And.Contain(p => p.Name == "ValidateAsStringAsync")
                ;
        }

        [TestMethod]
        public void Given_Null_Parameters_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            func = async () => await StringExtensions.ValidateAsync(null, null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await StringExtensions.ValidateAsync(payload, null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await StringExtensions.ValidateAsync(payload, validator.Object, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Validation_Error_When_ValidateAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var path = "default.json";

            Func<Task> func = async () => await StringExtensions.ValidateAsync(payload, validator.Object, path).ConfigureAwait(false);
            func.Should().Throw<SchemaValidationException>();
        }

        [TestMethod]
        public async Task Given_Validation_Result_When_ValidateAsync_Invoked_Then_It_Should_Return_Result()
        {
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var validated = await StringExtensions.ValidateAsync(payload, validator.Object, path).ConfigureAwait(false);

            validated.Should().BeTrue();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_ValidateAsStringAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var payload = "{ \"hello\": \"world\" }";
            var validator = new Mock<ISchemaValidator>();
            var func = default(Func<Task>);

            func = async () => await StringExtensions.ValidateAsStringAsync(null, null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await StringExtensions.ValidateAsStringAsync(payload, null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await StringExtensions.ValidateAsStringAsync(payload, validator.Object, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Validation_Error_When_ValidateAsStringAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var payload = "{ \"hello\": \"world\" }";

            var exception = new SchemaValidationException();
            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var path = "default.json";

            Func<Task> func = async () => await StringExtensions.ValidateAsStringAsync(payload, validator.Object, path).ConfigureAwait(false);
            func.Should().Throw<SchemaValidationException>();
        }

        [TestMethod]
        public async Task Given_Validation_Result_When_ValidateAsStringAsync_Invoked_Then_It_Should_Return_Result()
        {
            var payload = "{ \"hello\": \"world\" }";

            var validator = new Mock<ISchemaValidator>();
            validator.Setup(p => p.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var path = "default.json";

            var validated = await StringExtensions.ValidateAsStringAsync(payload, validator.Object, path).ConfigureAwait(false);

            validated.Should().Be(payload);
        }
    }
}
