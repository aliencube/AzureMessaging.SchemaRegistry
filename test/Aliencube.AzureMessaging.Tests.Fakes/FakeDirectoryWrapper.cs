using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

namespace Aliencube.AzureMessaging.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeDirectoryWrapper : IDirectoryWrapper
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }
    }
}
