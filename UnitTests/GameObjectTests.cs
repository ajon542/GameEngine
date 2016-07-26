using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;

namespace UnitTests
{
    [TestClass]
    public class GameObjectTests
    {
        [TestMethod]
        public void TestComponentAddition()
        {
            Component mesh = new Mesh();
            GameObject go = new GameObject();

            Assert.IsNull(go.GetComponent<Mesh>());
            go.AddComponent<Mesh>(mesh);
            Assert.IsNotNull(go.GetComponent<Mesh>());
        }

        [TestMethod]
        public void TestComponentRemoval()
        {
            // Create the components and game object.
            Component mesh = new Mesh();
            Component renderer = new Renderer();
            Component behaviour = new Behaviour();
            GameObject go = new GameObject();

            // Make sure the game object doesn't have the components.
            Assert.IsNull(go.GetComponent<Mesh>());
            Assert.IsNull(go.GetComponent<Renderer>());
            Assert.IsNull(go.GetComponent<Behaviour>());

            // Add the components to the game object.
            go.AddComponent<Mesh>(mesh);
            go.AddComponent<Renderer>(renderer);
            go.AddComponent<Behaviour>(behaviour);

            // Make sure components are found.
            Assert.IsNotNull(go.GetComponent<Mesh>());
            Assert.IsNotNull(go.GetComponent<Renderer>());
            Assert.IsNotNull(go.GetComponent<Behaviour>());

            // Make sure components correct components exist after removal.
            go.RemoveComponent<Mesh>();
            Assert.IsNull(go.GetComponent<Mesh>());
            Assert.IsNotNull(go.GetComponent<Renderer>());
            Assert.IsNotNull(go.GetComponent<Behaviour>());

            go.RemoveComponent<Renderer>();
            Assert.IsNull(go.GetComponent<Renderer>());
            Assert.IsNotNull(go.GetComponent<Behaviour>());

            go.RemoveComponent<Behaviour>();
            Assert.IsNull(go.GetComponent<Behaviour>());
        }

        [TestMethod]
        public void TestGameObjectManagerChildAddition()
        {
            GameObject go = new GameObject();

            GameObject c1 = new GameObject();
            GameObject cc1 = new GameObject();
            GameObject cc2 = new GameObject();
            c1.AddChild(cc1);
            c1.AddChild(cc2);

            GameObject c2 = new GameObject();
            GameObject c3 = new GameObject();

            go.AddChild(c1);
            go.AddChild(c2);
            go.AddChild(c3);

            // Check that all children are found.
            Assert.IsNotNull(GameObjectHierarchy.FindGameObject(go, c1.Guid));
            Assert.IsNotNull(GameObjectHierarchy.FindGameObject(go, cc1.Guid));
            Assert.IsNotNull(GameObjectHierarchy.FindGameObject(go, cc2.Guid));
            Assert.IsNotNull(GameObjectHierarchy.FindGameObject(go, c2.Guid));
            Assert.IsNotNull(GameObjectHierarchy.FindGameObject(go, c3.Guid));

            // Check that a child that was not added, is not found.
            Assert.IsNull(GameObjectHierarchy.FindGameObject(go, Guid.NewGuid()));
        }

        [TestMethod]
        public void TestGameObjectManagerChildRemoval()
        {
            GameObject go = new GameObject();

            GameObject c1 = new GameObject();
            GameObject cc1 = new GameObject();
            GameObject cc2 = new GameObject();
            c1.AddChild(cc1);
            c1.AddChild(cc2);
            go.AddChild(c1);

            // Remove the children.
            c1.RemoveChildren();

            // Check that children aren't found.
            Assert.IsNull(GameObjectHierarchy.FindGameObject(go, cc1.Guid));
            Assert.IsNull(GameObjectHierarchy.FindGameObject(go, cc2.Guid));

            // Remove the children.
            go.RemoveChildren();
            Assert.IsNull(GameObjectHierarchy.FindGameObject(go, c1.Guid));
        }

        [TestMethod]
        public void TestGameObjectParentNotNull()
        {
            GameObject go = new GameObject();
            GameObject c1 = new GameObject();
            go.AddChild(c1);

            Assert.IsNull(go.GetParent());
            Assert.IsNotNull(c1.GetParent());
            Assert.AreEqual(go.Guid, c1.GetParent().Guid);
        }

        [TestMethod]
        public void TestGameObjectLevel()
        {
            GameObject go = new GameObject("go");
            GameObject c1 = new GameObject("c1");
            GameObject cc1 = new GameObject("cc1");
            GameObject cc2 = new GameObject("cc2");
            c1.AddChild(cc1);
            c1.AddChild(cc2);
            go.AddChild(c1);

            // Root object is level 0.
            Assert.AreEqual(0, GameObjectHierarchy.GetLevel(go));

            Assert.AreEqual(1, GameObjectHierarchy.GetLevel(c1));
            Assert.AreEqual(2, GameObjectHierarchy.GetLevel(cc1));
            Assert.AreEqual(2, GameObjectHierarchy.GetLevel(cc2));
        }

        [TestMethod]
        public void TestGameObjectRoot()
        {
            GameObject go = new GameObject();
            GameObject c1 = new GameObject("c1");
            GameObject cc1 = new GameObject("cc1");
            GameObject cc2 = new GameObject("cc2");
            c1.AddChild(cc1);
            c1.AddChild(cc2);
            go.AddChild(c1);

            Assert.AreEqual(go.Guid, GameObjectHierarchy.GetRoot(go).Guid);
            Assert.AreEqual(go.Guid, GameObjectHierarchy.GetRoot(c1).Guid);
            Assert.AreEqual(go.Guid, GameObjectHierarchy.GetRoot(cc1).Guid);
            Assert.AreEqual(go.Guid, GameObjectHierarchy.GetRoot(cc2).Guid);
        }

        [TestMethod]
        public void TestClosestCommonParentWithNoCommonRoot()
        {
            GameObject go1 = new GameObject();
            GameObject c1 = new GameObject();
            go1.AddChild(c1);
            GameObject go2 = new GameObject();
            GameObject c2 = new GameObject();
            go2.AddChild(c2);

            // The game objects do not have same root, null expected.
            Assert.IsNull(GameObjectHierarchy.FindClosestCommonParent(c1, c2));
        }

        [TestMethod]
        public void TestClosestCommonParentWithGameObjectRoot()
        {
            GameObject go = new GameObject("go");
            GameObject c1 = new GameObject("c1");
            go.AddChild(c1);

            Assert.AreEqual("go", GameObjectHierarchy.FindClosestCommonParent(go, c1).Name);
        }

        [TestMethod]
        public void TestClosestCommonParent()
        {
            GameObject go = new GameObject("go");
            GameObject c1 = new GameObject("c1");
            GameObject c2 = new GameObject("c2");
            GameObject cc1 = new GameObject("cc1");
            GameObject cc2 = new GameObject("cc2");
            GameObject cc3 = new GameObject("cc3");
            GameObject cc4 = new GameObject("cc4");
            GameObject cc5 = new GameObject("cc5");
            GameObject cc6 = new GameObject("cc6");

            cc5.AddChild(cc6);
            cc4.AddChild(cc5);
            cc3.AddChild(cc4);

            cc1.AddChild(cc3);
            c1.AddChild(cc1);
            c1.AddChild(cc2);
            go.AddChild(c1);
            go.AddChild(c2);

            Assert.AreEqual("go", GameObjectHierarchy.FindClosestCommonParent(c2, cc6).Name);
            Assert.AreEqual("go", GameObjectHierarchy.FindClosestCommonParent(c1, c2).Name);
            Assert.AreEqual("c1", GameObjectHierarchy.FindClosestCommonParent(cc1, cc2).Name);
            Assert.AreEqual("c1", GameObjectHierarchy.FindClosestCommonParent(cc3, cc2).Name);
            Assert.AreEqual("cc4", GameObjectHierarchy.FindClosestCommonParent(cc5, cc4).Name);
        }
    }
}
