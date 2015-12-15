using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
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

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };

            string output = JsonConvert.SerializeObject(root, Formatting.Indented, settings);

            Console.WriteLine(output);

            // This almost deserializes correctly. We would just need to traverse the tree
            // and set all the "parent" references.
            GameObject loadedRoot = JsonConvert.DeserializeObject<GameObject>(output, settings);
        }
    }
}
