using System.Diagnostics.CodeAnalysis;
using System.IO;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers
{
    /// <summary>
    /// This represents the wrapper entity for <see cref="Directory"/>.
    /// </summary>
    [SuppressMessage("Style", "IDE0022:Use expression body for methods")]
    public class DirectoryWrapper : IDirectoryWrapper
    {
        /// <inheritdoc />
        public DirectoryInfo CreateDirectory(string path)
        {
            path.ThrowIfNullOrWhiteSpace();

            return Directory.CreateDirectory(path);
        }
    }
}
