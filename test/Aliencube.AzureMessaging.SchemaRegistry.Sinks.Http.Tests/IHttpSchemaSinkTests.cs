using System;
using System.Net.Http;
using System.Text;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Tests
{
    [TestClass]
    public class IHttpSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseInterface()
        {
            typeof(IHttpSchemaSink).Should().Implement<ISchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(IHttpSchemaSink)
                .Should().HaveProperty<Encoding>("Encoding")
                    .Which.Should().BeReadable()
                        .And.BeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(IHttpSchemaSink)
                .Should().HaveMethod("WithBaseLocation", new[] { typeof(Uri) })
                    .Which.Should().Return<ISchemaSink>();

            typeof(IHttpSchemaSink)
                .Should().HaveMethod("WithHttpClient", new[] { typeof(HttpClient) })
                    .Which.Should().Return<ISchemaSink>();
        }
    }
}
