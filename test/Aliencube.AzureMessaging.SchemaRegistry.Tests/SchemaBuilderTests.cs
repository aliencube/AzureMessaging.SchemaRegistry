using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;
using Aliencube.AzureMessaging.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

using NJsonSchema;
using NJsonSchema.Generation;

namespace Aliencube.AzureMessaging.SchemaRegistry.Tests
{
    [TestClass]
    public class SchemaBuilderTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(SchemaBuilder).Should().Implement<ISchemaBuilder>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(SchemaBuilder)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(JsonSchemaGeneratorSettings) });
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(SchemaBuilder)
                .Should().HaveProperty<JsonSchemaGeneratorSettings>("Settings")
                    .Which.Should().BeReadable()
                          .And.BeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(SchemaBuilder)
                .Should().HaveMethod("WithSettings", new[] { typeof(JsonSchemaGeneratorSettings) })
                    .Which.Should().Return<ISchemaBuilder>();

            typeof(SchemaBuilder)
                .Should().HaveMethod("Build", new[] { typeof(Type) })
                    .Which.Should().Return<JsonSchema>();

            typeof(SchemaBuilder)
                .Should().HaveMethod("Build", new List<Type>())
                    .Which.Should().Return<JsonSchema>();

            typeof(SchemaBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.IsGenericMethodDefinition)
                                 .SingleOrDefault(p => p.Name.IsEquivalentTo("Build", StringComparison.CurrentCulture))
                .Should().NotBeNull()
                .And.Return<JsonSchema>()
                .And.Subject.GetParameters()
                    .Should().BeEmpty()
                ;
        }

        [TestMethod]
        public void Given_DefaultValue_When_Instantiated_Then_It_Should_Return_Null_Values()
        {
            var instance = new SchemaBuilder();

            instance.Settings.Should().BeNull();
        }

        [TestMethod]
        public void Given_Settings_When_Instantiated_Then_It_Should_Return_Values()
        {
            var settings = new JsonSchemaGeneratorSettings();
            var instance = new SchemaBuilder(settings);

            instance.Settings
                .Should().NotBeNull()
                .And.Be(settings);
        }

        [TestMethod]
        public void Given_Null_When_WithSettings_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaBuilder()
                                      .WithSettings(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_When_WithSettings_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new SchemaBuilder()
                                      .WithSettings(default(JsonSchemaGeneratorSettings));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Settings_When_WithSettings_Invoked_Then_It_Should_Return_Values()
        {
            var settings = new JsonSchemaGeneratorSettings();
            var instance = new SchemaBuilder()
                               .WithSettings(settings);

            instance.Settings
                .Should().NotBeNull()
                .And.Be(settings);
        }

        [TestMethod]
        public void Given_Null_When_Build_Invoked_Then_It_Should_Throw_Exception()
        {
            var settings = new JsonSchemaGeneratorSettings();

            Action action = () => new SchemaBuilder()
                                      .WithSettings(settings)
                                      .Build(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Default_When_Build_Invoked_Then_It_Should_Throw_Exception()
        {
            var settings = new JsonSchemaGeneratorSettings();

            Action action = () => new SchemaBuilder()
                                      .WithSettings(settings)
                                      .Build(default(Type));

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Type_When_Build_Invoked_Then_It_Should_Return_Values()
        {
            var settings = new JsonSchemaGeneratorSettings();
            var result = new SchemaBuilder()
                             .WithSettings(settings)
                             .Build(typeof(FakeClass));

            var schema = JObject.Parse(result.ToJson());

            schema["properties"].As<JObject>()
                .Should().ContainKey("FakeMessage");
        }

        [TestMethod]
        public void Given_TypeParameter_When_Build_Invoked_Then_It_Should_Return_Values()
        {
            var settings = new JsonSchemaGeneratorSettings();
            var result = new SchemaBuilder()
                             .WithSettings(settings)
                             .Build<FakeClass>();

            var schema = JObject.Parse(result.ToJson());

            schema["properties"].As<JObject>()
                .Should().ContainKey("FakeMessage");

            var token = schema["properties"].As<JObject>()
                .Should().ContainKey("FakeMessage")
                    .WhichValue.SelectToken("type").SingleOrDefault(p => p.Value<string>().IsEquivalentTo("string", StringComparison.CurrentCulture))
                        .Should().NotBeNull();
        }

        [TestMethod]
        public void Given_SampleClass_When_Build_Invoked_Then_It_Should_Return_Values()
        {
            var settings = new JsonSchemaGeneratorSettings();
            var result = new SchemaBuilder()
                             .WithSettings(settings)
                             .Build<SampleClass>();

            var schema = result.ToJson();
        }
    }
}
