
namespace GameEngine.Core.Utilities
{
    /// <summary>
    /// Abstract base type class for type parsers.
    /// </summary>
    public abstract class BaseType : ITypeParser
    {
        /// <summary>
        /// Gets the id of the type to be parsed.
        /// </summary>
        protected abstract string Id { get; }

        /// <summary>
        /// Determines whether the type can be parsed.
        /// </summary>
        /// <param name="id">The identifier for the type.</param>
        /// <returns>true if the type can be parsed; false otherwise</returns>
        public bool CanParse(string id)
        {
            return id == Id;
        }

        /// <summary>
        /// Parse the input.
        /// </summary>
        /// <param name="input">The input containing the type data.</param>
        public abstract void Parse(string id);
    }
}
