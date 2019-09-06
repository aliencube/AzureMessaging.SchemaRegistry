using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aliencube.AzureMessaging.SchemaRegistry.FunctionAppV2
{
    [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider")]
    internal static class Utility
    {
        internal static string GetBasePath()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var segments = location.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var basePath = string.Join(Path.DirectorySeparatorChar.ToString(), segments.Take(segments.Count - 2));

            return basePath;
        }
    }
}
