using System.Threading.Tasks;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks
{
    /// <summary>
    /// This provides interfaces to <see cref="SchemaSink"/>.
    /// </summary>
    public interface ISchemaSink
    {
        /// <summary>
        /// Gets or sets the unique name of the sink.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the base sink location.
        /// </summary>
        string BaseLocation { get; }

        /// <summary>
        /// Adds base sink location.
        /// </summary>
        /// <param name="location">Base sink location.</param>
        /// <returns>Returns <see cref="ISchemaSink"/> instance.</returns>
        ISchemaSink WithBaseLocation(string location);

        /// <summary>
        /// Gets the JSON schema.
        /// </summary>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns the JSON schema.</returns>
        Task<string> GetSchemaAsync(string path);

        /// <summary>
        /// Sets the schema.
        /// </summary>
        /// <param name="schema">JSON schema.</param>
        /// <param name="path">Schema path.</param>
        /// <returns>Returns <c>True</c>, if successful; otherwise returns <c>False</c>.</returns>
        Task<bool> SetSchemaAsync(string schema, string path);
    }
}
