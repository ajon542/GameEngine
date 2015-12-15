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
            GameObject c2 = new GameObject("C2");
            GameObject c3 = new GameObject("C3");

            root.AddChild(c1);
            root.AddChild(c2);
            root.AddChild(c3);

            string output = JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // This almost deserializes correctly. We would just need to traverse the tree
            // and set all the "parent" references.
            GameObject loadedRoot = JsonConvert.DeserializeObject<GameObject>(output, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
