using System;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests.Extensions
{
    [TestClass]
    public class GenericExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(GenericExtensions).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name.IsEquivalentTo("IsNullOrDefault", StringComparison.CurrentCulture))
                .And.Contain(p => p.Name.IsEquivalentTo("ThrowIfNullOrDefault", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Value_When_IsNullOrDefault_Invoked_Then_It_Should_Return_True()
        {
            var value = (object)null;

            var result = GenericExtensions.IsNullOrDefault(value);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_Default_Value_When_IsNullOrDefault_Invoked_Then_It_Should_Return_True()
        {
            var value1 = default(object);

            var result1 = GenericExtensions.IsNullOrDefault(value1);
            result1.Should().BeTrue();

            var value2 = default(int);

            var result2 = GenericExtensions.IsNullOrDefault(value2);
            result2.Should().BeTrue();
        }

        [TestMethod]
        public void Given_Value_When_IsNullOrDefault_Invoked_Then_It_Should_Return_False()
        {
            var value = new object();

            var result = GenericExtensions.IsNullOrDefault(value);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_Null_Value_When_ThrowIfNullOrDefault_Invoked_Then_It_Should_Throw_Exception()
        {
            var value = (object)null;

            Action action = () => GenericExtensions.ThrowIfNullOrDefault(value);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_Value_When_ThrowIfNullOrDefault_Invoked_Then_It_Should_Return_Result()
        {
            var value = new object();

            var result = GenericExtensions.ThrowIfNullOrDefault(value);

            result
                .Should().NotBeNull()
                .And.BeOfType<object>();
        }
    }
}
