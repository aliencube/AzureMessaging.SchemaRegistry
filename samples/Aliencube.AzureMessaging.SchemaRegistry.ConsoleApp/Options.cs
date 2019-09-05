using System.Diagnostics.CodeAnalysis;

using CommandLine;

namespace Aliencube.AzureMessaging.SchemaRegistry.ConsoleApp
{
    [ExcludeFromCodeCoverage]
    public class Options
    {
        [Option('l', "base-location", Required = true, HelpText = "Base location of FileSystemSchemaSink")]
        public virtual string BaseLocation { get; set; }

        [Option('p', "file-path", Required = true, HelpText = "Path of the file to be produced")]
        public virtual string Filepath { get; set; }
    }
}
