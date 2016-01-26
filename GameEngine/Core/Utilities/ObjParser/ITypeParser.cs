
namespace GameEngine.Core.Utilities.ObjParser
{
    /// <summary>
    /// Interface for type parsers for the wavefront .obj file.
    /// </summary>
    interface ITypeParser
    {
        /// <summary>
        /// Determines whether the type can be parsed.
        /// </summary>
        /// <param name="id">The identifier for the type.</param>
        /// <returns>true if the type can be parsed; false otherwise</returns>
        bool CanParse(string id);

        /// <summary>
        /// Parse the input.
        /// </summary>
        /// <param name="input">The input containing the type data.</param>
        void Parse(string input);
    }
}
