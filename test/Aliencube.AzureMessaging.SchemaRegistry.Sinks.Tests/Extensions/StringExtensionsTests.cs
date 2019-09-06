using System;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Extensions.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(StringExtensions).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name == "IsNullOrWhiteSpace")
                .And.Contain(p => p.Name == "ThrowIfNullOrWhiteSpace")
                .And.Contain(p => p.Name == "IsEquivalentTo")
                .And.Contain(p => p.Name == "StartsWithEquivalentOf")
                .And.Contain(p => p.Name == "EndsWithEquivalentOf")
                ;
        }

        [TestMethod]
        public void Given_IsNullOrWhiteSpace_Method_When_Null_Value_Passed_Then_It_Should_Return_True()
        {
            var value = (string)null;

            var result = StringExtensions.IsNullOrWhiteSpace(value);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_IsNullOrWhiteSpace_Method_When_Empty_Value_Passed_Then_It_Should_Return_True()
        {
            var value = string.Empty;

            var result = StringExtensions.IsNullOrWhiteSpace(value);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_IsNullOrWhiteSpace_Method_When_Value_Passed_Then_It_Should_Return_False()
        {
            var value = "hello world";

            var result = StringExtensions.IsNullOrWhiteSpace(value);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_ThrowIfNullOrWhiteSpace_Method_When_Null_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = (string)null;

            Action action = () => StringExtensions.ThrowIfNullOrWhiteSpace(value);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_ThrowIfNullOrWhiteSpace_Method_When_Empty_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = string.Empty;

            Action action = () => StringExtensions.ThrowIfNullOrWhiteSpace(value);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_ThrowIfNullOrWhiteSpace_Method_When_Value_Passed_Then_It_Should_Return_Result()
        {
            var value = "hello world";

            var result = StringExtensions.ThrowIfNullOrWhiteSpace(value);

            result.Should().BeEquivalentTo(value);
        }

        [TestMethod]
        public void Given_IsEquivalentTo_Method_When_Null_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = (string)null;
            var comparer = (string)null;

            Action action = () => StringExtensions.IsEquivalentTo(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_IsEquivalentTo_Method_When_Empty_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = string.Empty;
            var comparer = (string)null;

            Action action = () => StringExtensions.IsEquivalentTo(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello world", null, StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "", StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "hello world", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "HELLO WORLD", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "HELLO WORLD", StringComparison.CurrentCulture, false)]
        [DataRow("hello world", "lorem-ipsum", StringComparison.CurrentCultureIgnoreCase, false)]
        public void Given_IsEquivalentTo_Method_When_Value_Passed_Then_It_Should_Return_Result(string value, string comparer, StringComparison stringComparison, bool expected)
        {
            var result = StringExtensions.IsEquivalentTo(value, comparer, stringComparison);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_StartsWithEquivalentOf_Method_When_Null_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = (string)null;
            var comparer = (string)null;

            Action action = () => StringExtensions.StartsWithEquivalentOf(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_StartsWithEquivalentOf_Method_When_Empty_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = string.Empty;
            var comparer = (string)null;

            Action action = () => StringExtensions.StartsWithEquivalentOf(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello world", null, StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "", StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "hello", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "HELLO", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "HELLO WORLD", StringComparison.CurrentCulture, false)]
        [DataRow("hello world", "lorem-ipsum", StringComparison.CurrentCultureIgnoreCase, false)]
        public void Given_StartsWithEquivalentOf_Method_When_Value_Passed_Then_It_Should_Return_Result(string value, string comparer, StringComparison stringComparison, bool expected)
        {
            var result = StringExtensions.StartsWithEquivalentOf(value, comparer, stringComparison);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_EndsWithEquivalentOf_Method_When_Null_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = (string)null;
            var comparer = (string)null;

            Action action = () => StringExtensions.EndsWithEquivalentOf(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_EndsWithEquivalentOf_Method_When_Empty_Value_Passed_Then_It_Should_Throw_Exception()
        {
            var value = string.Empty;
            var comparer = (string)null;

            Action action = () => StringExtensions.EndsWithEquivalentOf(value, comparer);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello world", null, StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "", StringComparison.CurrentCultureIgnoreCase, false)]
        [DataRow("hello world", "world", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "WORLD", StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow("hello world", "HELLO WORLD", StringComparison.CurrentCulture, false)]
        [DataRow("hello world", "lorem-ipsum", StringComparison.CurrentCultureIgnoreCase, false)]
        public void Given_EndsWithEquivalentOf_Method_When_Value_Passed_Then_It_Should_Return_Result(string value, string comparer, StringComparison stringComparison, bool expected)
        {
            var result = StringExtensions.EndsWithEquivalentOf(value, comparer, stringComparison);

            result.Should().Be(expected);
        }
    }
}
