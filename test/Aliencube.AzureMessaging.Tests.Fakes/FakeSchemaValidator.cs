using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;
using Aliencube.AzureMessaging.SchemaValidation;

namespace Aliencube.AzureMessaging.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeSchemaValidator : ISchemaValidator
    {
        public ISchemaSink Sink { get; }

        public Task<bool> ValidateAsync(string payload, string path)
        {
            throw new System.NotImplementedException();
        }

        public ISchemaValidator WithSink(ISchemaSink sink)
        {
            throw new System.NotImplementedException();
        }
    }
}
