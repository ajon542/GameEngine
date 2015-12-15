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
            
            // TODO: Probably need a Transform comparer.
            Assert.AreEqual(children.Count, 1);

            Assert.AreEqual(1, children[0].Transform.Position.X);
            Assert.AreEqual(2, children[0].Transform.Position.Y);
            Assert.AreEqual(3, children[0].Transform.Position.Z);

            Assert.AreEqual(4, children[0].Transform.Scale.X);
            Assert.AreEqual(5, children[0].Transform.Scale.Y);
            Assert.AreEqual(6, children[0].Transform.Scale.Z);

            Assert.AreEqual(7, children[0].Transform.Rotation.X);
            Assert.AreEqual(8, children[0].Transform.Rotation.Y);
            Assert.AreEqual(9, children[0].Transform.Rotation.Z);
            Assert.AreEqual(10, children[0].Transform.Rotation.W);
        }
    }
}
