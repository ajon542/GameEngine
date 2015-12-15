using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace GameEngine.Core.Serialization
{
    public class Serializer : ISerializer
    {
        private JsonSerializer serializer;

        public Serializer()
        {
            // Create the serializer settings.
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };

            // Add the converters.
            serializer = JsonSerializer.Create(settings);

            serializer.Converters.Add(new VectorConverter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Formatting = Formatting.Indented;
        }

        public string Serialize(GameObject gameObject)
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

        public GameObject Deserialize(string data)
        {
            using (StringReader sr = new StringReader(data))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<GameObject>(reader);
                }
            }
        }
    }
}
