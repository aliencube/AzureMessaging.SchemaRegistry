using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;
using Aliencube.AzureMessaging.SchemaValidation;

namespace Aliencube.AzureMessaging.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeSchemaValidator : ISchemaValidator
    {
        public ISchemaConsumer Consumer { get; }

        public Task<bool> ValidateAsync(string payload, string path)
        {
            throw new System.NotImplementedException();
        }

        public ISchemaValidator WithSchemaConsumer(ISchemaConsumer consumer)
        {
            throw new System.NotImplementedException();
        }
    }
}
