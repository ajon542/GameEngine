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
        // TODO: Implement all the standard type converters as defined in:
        // https://github.com/opentk/opentk/tree/develop/Source/OpenTK/Math
        [TestMethod]
        public void TestVectorSerialization()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            ISerializer<Vector3> serializer = new Serializer<Vector3>();
            string output = serializer.Serialize(vec);
            Vector3 res = serializer.Deserialize<Vector3>(output);
            Assert.AreEqual(vec, res);
        }

        [TestMethod]
        public void TestQuaternionSerialization()
        {
            Quaternion qt = new Quaternion(1, 2, 3, 4);
            ISerializer<Quaternion> serializer = new Serializer<Quaternion>();
            string output = serializer.Serialize(qt);
            Quaternion res = serializer.Deserialize<Quaternion>(output);
            Assert.AreEqual(qt, res);
        }

        [TestMethod]
        public void TestMethod1()
        {
            GameObject root = new GameObject("Root");
            GameObject c1 = new GameObject("C1");
            c1.Transform.Position = new Vector3(1, 2, 3);
            c1.Transform.Scale = new Vector3(4, 5, 6);
            c1.Transform.Rotation = new Quaternion(7, 8, 9, 10);

            root.AddChild(c1);

            ISerializer<GameObject> serializer = new Serializer<GameObject>();

            string output = serializer.Serialize(root);
            GameObject loadedRoot = serializer.Deserialize<GameObject>(output);

            List<GameObject> children = loadedRoot.GetChildren();
            
            Assert.AreEqual(children.Count, 1);
            Assert.AreEqual(c1.Transform, children[0].Transform);
        }
    }
}
