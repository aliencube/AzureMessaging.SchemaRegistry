using System;
using System.Net.Http;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests
{
    [TestClass]
    public class HttpMethodExtensionsTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            var methods = typeof(HttpMethodExtensions).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            methods
                .Should().Contain(p => p.Name.IsEquivalentTo("IsSupported", StringComparison.CurrentCulture))
                ;
        }

        [TestMethod]
        public void Given_Null_Parameter_When_IsSupported_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => HttpMethodExtensions.IsSupported(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("GET", true)]
        [DataRow("POST", true)]
        [DataRow("PUT", true)]
        [DataRow("PATCH", true)]
        [DataRow("DELETE", false)]
        [DataRow("HEAD", false)]
        [DataRow("OPTIONS", false)]
        [DataRow("TRACE", false)]
        public void Given_Parameter_When_IsSupported_Invoked_Then_It_Should_Return_Result(string verb, bool expected)
        {
            var method = new HttpMethod(verb);

            var result = HttpMethodExtensions.IsSupported(method);

            result.Should().Be(expected);
        }
    }
}
