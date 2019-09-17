using System.Diagnostics.CodeAnalysis;

using CommandLine;

namespace Aliencube.AzureMessaging.SchemaValidation.ConsoleAppNet
{
    [ExcludeFromCodeCoverage]
    public class Options
    {
        [Option("blob-connectionstring", Required = true, HelpText = "Connection string for Azure Blob Storage")]
        public virtual string BlobConnectionString { get; set; }

        [Option("blob-baseuri", Required = true, HelpText = "Base URI of Azure Blob Storage")]
        public virtual string BlobBaseUri { get; set; }

        [Option("container", Required = true, HelpText = "Container of Azure Blob Storage")]
        public virtual string Container { get; set; }

        [Option("file-path", Required = true, HelpText = "Path of the file to be produced/consumed")]
        public virtual string Filepath { get; set; }

        [Option("servicebus-connectionstring", Required = true, HelpText = "Connection string for Azure Service Bus")]
        public virtual string ServiceBusConnectionString { get; set; }

        [Option("topic", Required = true, HelpText = "Topic of Azure Service Bus")]
        public virtual string Topic { get; set; }

        [Option("subscription", Required = true, HelpText = "Topic Subscription of Azure Service Bus")]
        public virtual string Subscription { get; set; }
    }
}
