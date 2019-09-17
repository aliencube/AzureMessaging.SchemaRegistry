using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aliencube.AzureMessaging.SchemaValidation.ConsoleAppNet
{
    [ExcludeFromCodeCoverage]
    public class SampleClass
    {
        public virtual string Text { get; set; } = string.Empty;

        public virtual Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();

        public virtual List<string> List { get; set; } = new List<string>();

        public virtual SampleEnum Enum { get; set; }

        public virtual SampleSubClass SubClass { get; set; } = new SampleSubClass();
    }

    [ExcludeFromCodeCoverage]
    public class SampleSubClass
    {
        public virtual int Number { get; set; }
    }

    public enum SampleEnum
    {
        Value1,
        Value2
    }
}
