using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NJsonSchema;
using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class ISchemaBuilderTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(ISchemaBuilder)
                .Should().HaveProperty<JsonSchemaGeneratorSettings>("Settings")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(ISchemaBuilder)
                .Should().HaveMethod("WithSettings", new[] { typeof(JsonSchemaGeneratorSettings) })
                    .Which.Should().Return<ISchemaBuilder>();

            typeof(ISchemaBuilder)
                .Should().HaveMethod("Build", new[] { typeof(Type) })
                    .Which.Should().Return<JsonSchema>();

            typeof(ISchemaBuilder)
                .Should().HaveMethod("Build", new List<Type>())
                    .Which.Should().Return<JsonSchema>();

            typeof(ISchemaBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.IsGenericMethodDefinition)
                                  .SingleOrDefault(p => p.Name.IsEquivalentTo("Build", StringComparison.CurrentCulture))
                .Should().NotBeNull()
                .And.Return<JsonSchema>()
                .And.Subject.GetParameters()
                    .Should().BeEmpty()
                ;
        }
    }
}
