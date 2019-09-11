using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class ISchemaProducerTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(ISchemaProducer)
                .Should().HaveProperty<ISchemaBuilder>("Builder")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();

            typeof(ISchemaProducer)
                .Should().HaveProperty<List<ISchemaSink>>("Sinks")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(ISchemaProducer)
                .Should().HaveMethod("WithBuilder", new[] { typeof(ISchemaBuilder) })
                    .Which.Should().Return<ISchemaProducer>();

            typeof(ISchemaProducer)
                .Should().HaveMethod("WithSink", new[] { typeof(ISchemaSink) })
                    .Which.Should().Return<ISchemaProducer>();

            typeof(ISchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().Return<Task<bool>>()
                    ;

            typeof(ISchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(Type), typeof(string) })
                    .Which.Should().Return<Task<bool>>()
                    ;

            typeof(ISchemaProducer)
                .Should().HaveMethod("ProduceAsync", new[] { typeof(string) })
                    .Which.Should().Return<Task<bool>>();

            typeof(ISchemaProducer).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => p.IsGenericMethodDefinition)
                                   .SingleOrDefault(p => p.Name.IsEquivalentTo("ProduceAsync", StringComparison.CurrentCulture))
                .Should().NotBeNull()
                .And.Return<Task<bool>>()
                .And.Subject.GetParameters()
                    .Should().HaveCount(1)
                ;
        }
    }
}
