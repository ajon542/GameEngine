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
        /// <summary>
        /// Private base class used in the serialization tests.
        /// </summary>
        private class Base
        {
            public int Value { get; set; }
        }

        /// <summary>
        /// Private derived class used in the serialization tests.
        /// </summary>
        private class Derived : Base
        {
            public string Name { get; set; }
        }

        // TODO: Implement all the standard type converters as defined in:
        // https://github.com/opentk/opentk/tree/develop/Source/OpenTK/Math
        [TestMethod]
        public void TestVector2Serialization()
        {
            Vector2 vec = new Vector2(1, 2);
            ISerializer<Vector2> serializer = new Serializer<Vector2>();
            string output = serializer.Serialize(vec);
            Vector2 res = serializer.Deserialize(output);
            Assert.AreEqual(vec, res);
        }

        [TestMethod]
        public void TestVector3Serialization()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            ISerializer<Vector3> serializer = new Serializer<Vector3>();
            string output = serializer.Serialize(vec);
            Vector3 res = serializer.Deserialize(output);
            Assert.AreEqual(vec, res);
        }

        [TestMethod]
        public void TestQuaternionSerialization()
        {
            Quaternion qt = new Quaternion(1, 2, 3, 4);
            ISerializer<Quaternion> serializer = new Serializer<Quaternion>();
            string output = serializer.Serialize(qt);
            Quaternion res = serializer.Deserialize(output);
            Assert.AreEqual(qt, res);
        }

        [TestMethod]
        public void TestDerivedSerialization()
        {
            Base b = new Derived { Value = 1234, Name = "Derived" };
            ISerializer<Base> serializer = new Serializer<Base>();
            string output = serializer.Serialize(b);
            Base res = serializer.Deserialize(output);
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
            GameObject loadedRoot = serializer.Deserialize(output);

            List<GameObject> children = loadedRoot.GetChildren();

            Assert.AreEqual(children.Count, 1);
            Assert.AreEqual(c1.Transform, children[0].Transform);
        }
    }
}
