using System.IO;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers
{
    /// <summary>
    /// This represents the wrapper entity for <see cref="File"/>.
    /// </summary>
    public class FileWrapper : IFileWrapper
    {
        /// <inheritdoc />
        public bool Exists(string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return false;
            }

            return File.Exists(path);
        }
    }
}
