using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace GameEngine.Core.Serialization
{
    public class Serializer<T> : ISerializer<T>
    {
        private JsonSerializer serializer;

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
            serializer.Formatting = Formatting.Indented;
        }

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
