﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            // NOTE: We can deserialize a string without knowing the derived object.
            // Pretty cool, huh? Thanks newtonsoft json.
            Base res = serializer.Deserialize(output);

            // Serialize the deserialized object.
            string compare = serializerCompare.Serialize(res);

            // Compare the resulting strings for equality.
            Assert.AreEqual(string.Compare(output, compare), 0);

            // Check the deserialized values are correct.
            Assert.AreEqual(res.Value, 1234);
            Assert.AreEqual((res as Derived).Name, "Derived");
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

        [TestMethod]
        public void TestComponentSerialization()
        {
            // Create a game object and add a mesh component.
            Mesh mesh = new Mesh();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Mesh>(mesh);

            // Serialize and deserialize the original object.
            ISerializer<GameObject> serializer = new Serializer<GameObject>();
            string output = serializer.Serialize(gameObject);
            GameObject loadedGameObject = serializer.Deserialize(output);

            // Deserialized game object should have a mesh.
            Mesh resultMesh = loadedGameObject.GetComponent<Mesh>() as Mesh;
            Assert.IsNotNull(resultMesh);

            // The mesh reference to the game object should be the original.
            Assert.AreEqual(gameObject.Guid, resultMesh.GameObject.Guid);

            ISerializer<GameObject> serializeResult = new Serializer<GameObject>();
            string resultOutput = serializeResult.Serialize(loadedGameObject);

            // Compare the resulting strings for equality.
            Assert.AreEqual(string.Compare(output, resultOutput), 0);
        }

        private GameObject SerializeDeserialize()
        {
            // TODO: Implement this.
            return null;
        }
    }
}
