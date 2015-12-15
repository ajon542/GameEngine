using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
using GameEngine.Core.Serialization;
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
            c1.Transform.Position = new Vector3(1, 2, 3);
            c1.Transform.Scale = new Vector3(4, 5, 6);
            c1.Transform.Rotation = new Quaternion(7, 8, 9, 10);

            root.AddChild(c1);

            ISerializer serializer = new Serializer();

            string output = serializer.Serialize(root);
            GameObject loadedRoot = serializer.Deserialize(output);

            List<GameObject> children = loadedRoot.GetChildren();
            
            Assert.AreEqual(children.Count, 1);
            Assert.AreEqual(c1.Transform, children[0].Transform);
        }
    }
}
