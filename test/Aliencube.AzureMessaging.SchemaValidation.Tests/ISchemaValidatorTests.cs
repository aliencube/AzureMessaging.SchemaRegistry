using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aliencube.AzureMessaging.SchemaValidation.Tests
{
    [TestClass]
    public class ISchemaValidatorTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(ISchemaValidator)
                .Should().HaveProperty<ISchemaConsumer>("Consumer")
                    .Which.Should().BeReadable()
                          .And.NotBeWritable();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(ISchemaValidator)
                .Should().HaveMethod("WithSchemaConsumer", new[] { typeof(ISchemaConsumer) })
                    .Which.Should().Return<ISchemaValidator>();

            typeof(ISchemaValidator)
                .Should().HaveMethod("ValidateAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().Return<Task<bool>>();
        }
    }
}
