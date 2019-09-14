using System.Text;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks
{
    /// <summary>
    /// This provides interfaces to <see cref="FileSystemSchemaSink"/>.
    /// </summary>
    public interface IFileSystemSchemaSink : ISchemaSink
    {
        /// <summary>
        /// Gets or sets the <see cref="IDirectoryWrapper"/> instance.
        /// </summary>
        IDirectoryWrapper Directory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFileWrapper"/> instance.
        /// </summary>
        IFileWrapper File { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> value.
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
