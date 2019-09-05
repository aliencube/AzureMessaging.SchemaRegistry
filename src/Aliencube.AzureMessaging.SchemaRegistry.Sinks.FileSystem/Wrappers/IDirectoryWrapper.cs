using System.IO;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers
{
    /// <summary>
    /// This provides interfaces to <see cref="DirectoryWrapper"/>.
    /// </summary>
    public interface IDirectoryWrapper
    {
        /// <summary>
        /// Creates the directory with given path.
        /// </summary>
        /// <param name="path">Fully qualified directory path.</param>
        /// <returns>Returns <see cref="DirectoryInfo"/> instance.</returns>
        DirectoryInfo CreateDirectory(string path);
    }
}
