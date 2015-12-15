using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
using Newtonsoft.Json;
using OpenTK;

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

            // Create the serializer settings.
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            // Add the converters.
            JsonSerializer serializer = JsonSerializer.Create(settings);
            serializer.Converters.Add(new VectorConverter());
            serializer.Formatting = Formatting.Indented;

            // Serialize the object.
            serializer.Serialize(sw, root);

            string output = sw.ToString();

            JsonTextReader reader = new JsonTextReader(new StringReader(output));
            GameObject loadedRoot = serializer.Deserialize<GameObject>(reader);

            Console.WriteLine(output);
        }
    }
}
