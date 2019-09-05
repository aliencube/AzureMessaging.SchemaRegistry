using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aliencube.AzureMessaging.SchemaRegistry.ConsoleApp
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
    public class SampleClass
    {
        public virtual string Text { get; set; }

        public virtual Dictionary<string, string> Dictionary { get; set; }

        public virtual List<string> List { get; set; }

        public virtual SampleEnum Enum { get; set; }

        public virtual SampleSubClass SubClass { get; set; }
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
