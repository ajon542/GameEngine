using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
using GameEngine.Core.Serialization;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            GameObject root = new GameObject("Root");
            GameObject c1 = new GameObject("C1");

            root.AddChild(c1);

            // TODO: This needs to be cleaned up and put in its own class.
            // TODO: Need to test derived classes.

            // Create the serializer settings.
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };

            // Add the converters.
            JsonSerializer serializer = JsonSerializer.Create(settings);

            serializer.Converters.Add(new VectorConverter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Formatting = Formatting.Indented;

            string output;
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                // Serialize the object.
                serializer.Serialize(sw, root);
                output = sw.ToString();
            }

            using (StringReader sr = new StringReader(output))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    GameObject loadedRoot = serializer.Deserialize<GameObject>(reader);
                    Console.WriteLine(output);
                }
            }
        }
    }
}
