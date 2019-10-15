using System.IO;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;
using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks
{
    /// <summary>
    /// This represents the schema sink entity for file system.
    /// </summary>
    public class FileSystemSchemaSink : SchemaSink, IFileSystemSchemaSink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSchemaSink"/> class.
        /// </summary>
        public FileSystemSchemaSink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSchemaSink"/> class.
        /// </summary>
        /// <param name="location">Base directory of the file system.</param>
        public FileSystemSchemaSink(string location)
            : base(location)
        {
        }

        /// <inheritdoc />
        public virtual IDirectoryWrapper Directory { get; set; } = new DirectoryWrapper();

        /// <inheritdoc />
        public virtual IFileWrapper File { get; set; } = new FileWrapper();

        /// <inheritdoc />
        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <inheritdoc />
        public override async Task<string> GetSchemaAsync(string path)
        {
            path.ThrowIfNullOrWhiteSpace();

            var filepath = Path.Combine(this.BaseLocation, path);
            if (!this.File.Exists(filepath))
            {
                throw new FileNotFoundException();
            }

            using (var stream = new FileStream(filepath, FileMode.Open))
            using (var reader = new StreamReader(stream, this.Encoding))
            {
                var schema = await reader.ReadToEndAsync().ConfigureAwait(false);

                return schema;
            }
        }

        /// <inheritdoc />
        public override async Task<bool> SetSchemaAsync(string schema, string path)
        {
            schema.ThrowIfNullOrWhiteSpace();
            path.ThrowIfNullOrWhiteSpace();

            var filepath = Path.Combine(this.BaseLocation, path);
            var directory = Path.GetDirectoryName(filepath);
            this.Directory.CreateDirectory(directory);

            using (var stream = new FileStream(filepath, FileMode.Create))
            using (var writer = new StreamWriter(stream, this.Encoding))
            {
                await writer.WriteAsync(schema).ConfigureAwait(false);
            }

            return true;
        }
    }
}
