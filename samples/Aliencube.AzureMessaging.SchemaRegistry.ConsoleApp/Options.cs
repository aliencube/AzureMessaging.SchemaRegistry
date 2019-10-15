using System.Diagnostics.CodeAnalysis;

using CommandLine;

namespace Aliencube.AzureMessaging.SchemaRegistry.ConsoleApp
{
    [ExcludeFromCodeCoverage]
    public class Options
    {
        [Option("blob-connectionstring", Required = true, HelpText = "Connection string for Azure Blob Storage")]
        public virtual string BlobConnectionString { get; set; }

        [Option("blob-baseuri", Required = true, HelpText = "Base URI of Azure Blob Storage")]
        public virtual string BlobBaseUri { get; set; }

        [Option("blob-container", Required = true, HelpText = "Container of Azure Blob Storage")]
        public virtual string Container { get; set; }

        [Option("file-baselocation", Required = true, HelpText = "Base location of FileSystemSchemaSink")]
        public virtual string FileBaseLocation { get; set; }

        [Option("file-path", Required = true, HelpText = "Path of the file to be produced")]
        public virtual string Filepath { get; set; }
    }
}
