using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace GameEngine.Core.Serialization
{
    /// <summary>
    /// Implementation of the (de)serializer.
    /// </summary>
    /// <typeparam name="T">The type to serialize.</typeparam>
    public class Serializer<T> : ISerializer<T>
    {
        /// <summary>
        /// The json serializer object.
        /// </summary>
        private JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        public Serializer()
        {
            // Create the serializer settings.
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            };

            // Add the converters.
            serializer = JsonSerializer.Create(settings);

            serializer.Converters.Add(new Vector2Converter());
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Converters.Add(new Matrix3Converter());
            serializer.Converters.Add(new Matrix4Converter());
            serializer.Formatting = Formatting.Indented;
        }

        /// <summary>
        /// Serialize the object.
        /// </summary>
        /// <param name="gameObject">The object to serialize.</param>
        /// <returns>A string representing the serialized object.</returns>
        public string Serialize(T gameObject)
        {
            string output;
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                // Serialize the object.
                serializer.Serialize(sw, gameObject);
                output = sw.ToString();
            }

            return output;
        }

        /// <summary>
        /// Deserialized the data.
        /// </summary>
        /// <param name="data">A string representation of the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize(string data)
        {
            using (StringReader sr = new StringReader(data))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(reader);
                }
            }
        }
    }
}
