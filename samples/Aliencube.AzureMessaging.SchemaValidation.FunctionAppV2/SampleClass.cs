using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV2
{
    [ExcludeFromCodeCoverage]
    public class SampleClass
    {
        public virtual string Text { get; set; } = "hello world";

        public virtual Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>() { { "lorem", "ipsum" } };

        public virtual List<string> List { get; set; } = new List<string>() { "hello-world" };

        public virtual SampleEnum Enum { get; set; } = SampleEnum.Value1;

        public virtual SampleSubClass SubClass { get; set; } = new SampleSubClass();
    }

    [ExcludeFromCodeCoverage]
    public class SampleSubClass
    {
        public virtual int Number { get; set; } = 1;
    }

    public enum SampleEnum
    {
        Value1,
        Value2
    }
}
