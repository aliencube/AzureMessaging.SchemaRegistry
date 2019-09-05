using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeSchemaSink : SchemaSink
    {
        public FakeSchemaSink()
        {
        }

        public FakeSchemaSink(string location)
            : base(location)
        {
        }
    }
}
