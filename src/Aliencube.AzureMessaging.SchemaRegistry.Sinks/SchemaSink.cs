using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaRegistry.Sinks.Extensions;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks
{
    /// <summary>
    /// This represents the entity for schema sink that determines where the schema hosting location is.
    /// </summary>
    public abstract class SchemaSink : ISchemaSink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSink"/> class.
        /// </summary>
        protected SchemaSink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSink"/> class.
        /// </summary>
        /// <param name="location"></param>
        protected SchemaSink(string location)
        {
            this.BaseLocation = location.ThrowIfNullOrWhiteSpace();
        }

        /// <inheritdoc />
        public virtual string BaseLocation { get; protected set; } = string.Empty;

        /// <inheritdoc />
        public virtual ISchemaSink WithBaseLocation(string location)
        {
            this.BaseLocation = location.ThrowIfNullOrWhiteSpace();

            return this;
        }

        /// <inheritdoc />
        public virtual async Task<string> GetSchemaAsync(string path)
        {
            return await Task.FromResult((string)null).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<bool> SetSchemaAsync(string schema, string path)
        {
            return await Task.FromResult(true).ConfigureAwait(false);
        }
    }
}
