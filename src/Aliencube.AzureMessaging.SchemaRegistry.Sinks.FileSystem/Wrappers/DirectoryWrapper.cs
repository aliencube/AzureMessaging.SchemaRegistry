using System.IO;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers
{
    /// <summary>
    /// This represents the wrapper entity for <see cref="Directory"/>.
    /// </summary>
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
