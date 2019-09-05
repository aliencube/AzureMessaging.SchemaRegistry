namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Wrappers
{
    /// <summary>
    /// This provides interfaces to <see cref="FileWrapper"/>.
    /// </summary>
    public interface IFileWrapper
    {
        /// <summary>
        /// Checks whether the file exists at the given path.
        /// </summary>
        /// <param name="path">Fully qualified file path.</param>
        /// <returns>Returns <c>True</c>, if the file exists; otherwise returns <c>False</c>.</returns>
        bool Exists(string path);
    }
}
