using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaValidation.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Validates the payload against the schema from the sink.
        /// </summary>
        /// <param name="payload">JSON payload.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path in the sink.</param>
        /// <returns>Returns <c>True</c>, if the payload is valid; otherwise throws <see cref="SchemaValidationException"/>.</returns>
        public static async Task<bool> ValidateAsync(this string payload, ISchemaValidator validator, string path)
        {
            payload.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return validated;
        }

        /// <summary>
        /// Validates the payload against the schema from the sink.
        /// </summary>
        /// <param name="payload">JSON payload.</param>
        /// <param name="validator"><see cref="ISchemaValidator"/> instance.</param>
        /// <param name="path">Schema path in the sink.</param>
        /// <returns>Returns the payload, if the payload is valid; otherwise throws <see cref="SchemaValidationException"/>.</returns>
        public static async Task<string> ValidateAsStringAsync(this string payload, ISchemaValidator validator, string path)
        {
            payload.ThrowIfNullOrWhiteSpace();
            validator.ThrowIfNullOrDefault();
            path.ThrowIfNullOrWhiteSpace();

            var validated = await validator.ValidateAsync(payload, path).ConfigureAwait(false);

            return payload;
        }
    }
}
