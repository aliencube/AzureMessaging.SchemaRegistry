using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.FileSystem.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaRegistry.Sinks.Http.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.HttpClient")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.HttpClient.Tests")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.ServiceBus")]
[assembly: InternalsVisibleTo("Aliencube.AzureMessaging.SchemaValidation.ServiceBus.Tests")]

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions
{
    /// <summary>
    /// This represents the extension entity for generics.
    /// </summary>
    internal static class GenericExtensions
    {
        /// <summary>
        /// Checks whether the object is null or default, or not.
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <param name="object">Type object.</param>
        /// <returns>Returns <c>True</c>, if the object is null or default; otherwise returns <c>False</c>.</returns>
        internal static bool IsNullOrDefault<T>(this T @object)
        {
            if (@object == null)
            {
                return true;
            }

            if (@object.Equals(default(T)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the object is null or default. If the object is null or default, throws an exception.
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <param name="object">Type object.</param>
        /// <returns>Returns the original object, if it is NOT null or default.</returns>
        internal static T ThrowIfNullOrDefault<T>(this T @object)
        {
            if (@object.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(@object));
            }

            return @object;
        }
    }
}
