using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry;

namespace Aliencube.AzureMessaging.SchemaValidation
{
    /// <summary>
    /// This provides interfaces to <see cref="SchemaValidator"/>.
    /// </summary>
    public interface ISchemaValidator
    {
        /// <summary>
        /// Gets the <see cref="ISchemaConsumer"/> instance.
        /// </summary>
        ISchemaConsumer Consumer { get; }

        /// <summary>
        /// Adds the <see cref="ISchemaConsumer"/> to the validator.
        /// </summary>
        /// <param name="consumer"><see cref="ISchemaConsumer"/> instance.</param>
        /// <returns>Returns the <see cref="ISchemaValidator"/> instance.</returns>
        ISchemaValidator WithSchemaConsumer(ISchemaConsumer consumer);

        /// <summary>
        /// Validates the payload against the schema from the sink.
        /// </summary>
        /// <param name="payload">JSON payload.</param>
        /// <param name="path">Schema path in the sink.</param>
        /// <returns>Returns <c>True</c>, if the payload is valid; otherwise throws <see cref="SchemaNotFoundException"/>, <see cref="SchemaMalformedException"/> or <see cref="SchemaValidationException"/>.</returns>
        Task<bool> ValidateAsync(string payload, string path);
    }
}
