
namespace GameEngine.Core.Serialization
{
    /// <summary>
    /// Interface for (de)serialization.
    /// </summary>
    /// <remarks>
    /// The purpose of serialization is to be able to save and load the entire
    /// scene and associated game object hierarchy.
    /// </remarks>
    /// <typeparam name="T">The type to serialize.</typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serialize the object.
        /// </summary>
        /// <param name="gameObject">The object to serialize.</param>
        /// <returns>A string representing the serialized object.</returns>
        string Serialize(T gameObject);

        /// <summary>
        /// Deserialized the data.
        /// </summary>
        /// <param name="data">A string representation of the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize(string data);
    }
}
