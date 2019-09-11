using System;
using System.Diagnostics.CodeAnalysis;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

namespace Aliencube.AzureMessaging.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeFileWrapper : IFileWrapper
    {
        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }
    }
}
