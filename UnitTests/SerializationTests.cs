using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestDerivedSerialization()
        {
            // Create the serializers.
            ISerializer<Base> serializer = new Serializer<Base>();
            ISerializer<Base> serializerCompare = new Serializer<Base>();

            // Create the object.
            Base b = new Derived { Value = 1234, Name = "Derived" };        
  
            // Perform (de)serialization.
            string output = serializer.Serialize(b);
            Base res = serializer.Deserialize(output);

            // Serialize the deserialized object.
            string compare = serializerCompare.Serialize(res);

            // Compare the resulting strings for equality.
            Assert.AreEqual(string.Compare(output, compare), 0);
        }

        [TestMethod]
        public void TestGameObjectParentChild()
        {
            GameObject root = new GameObject("Root");
            GameObject c1 = new GameObject("C1");
            c1.Transform.Position = new Vector3(1, 2, 3);
            c1.Transform.Scale = new Vector3(4, 5, 6);
            c1.Transform.Rotation = new Quaternion(7, 8, 9, 10);

            root.AddChild(c1);

            ISerializer<GameObject> serializer = new Serializer<GameObject>();

            // Serialize and deserialize the original object.
            string output = serializer.Serialize(root);
            GameObject loadedRoot = serializer.Deserialize(output);

            List<GameObject> children = loadedRoot.GetChildren();

            Assert.AreEqual(children.Count, 1);
            Assert.AreEqual(c1.Transform, children[0].Transform);
            Assert.AreEqual(root.Guid, loadedRoot.Guid);
            Assert.AreEqual(c1.Guid, children[0].Guid);
        }
    }
}
