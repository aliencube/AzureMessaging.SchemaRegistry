using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This provides interfaces to <see cref="SchemaValidator"/>.
    /// </summary>
    public interface ISchemaValidator
    {
        /// <summary>
        /// Gets the <see cref="ISchemaSink"/> instance.
        /// </summary>
        ISchemaSink Sink { get; }

        /// <summary>
        /// Adds the <see cref="ISchemaSink"/> to the validator.
        /// </summary>
        /// <param name="sink"><see cref="ISchemaSink"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaValidator"/> instance.</returns>
        ISchemaValidator WithSink(ISchemaSink sink);

        /// <summary>
        /// Validates the payload against the schema from the sink.
        /// </summary>
        /// <param name="payload">JSON payload.</param>
        /// <param name="path">Schema path in the sink.</param>
        /// <returns>Returns <c>True</c>, if the payload is valid; otherwise throws <see cref="SchemaValidationException"/>.</returns>
        Task<bool> ValidateAsync(string payload, string path);
    }
}
