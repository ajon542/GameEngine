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
            Assert.IsNotNull(GameObjectManager.FindGameObject(go, c1.Guid));
            Assert.IsNotNull(GameObjectManager.FindGameObject(go, cc1.Guid));
            Assert.IsNotNull(GameObjectManager.FindGameObject(go, cc2.Guid));
            Assert.IsNotNull(GameObjectManager.FindGameObject(go, c2.Guid));
            Assert.IsNotNull(GameObjectManager.FindGameObject(go, c3.Guid));

            // Check that a child that was not added, is not found.
            Assert.IsNull(GameObjectManager.FindGameObject(go, Guid.NewGuid()));
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
            Assert.IsNull(GameObjectManager.FindGameObject(go, cc1.Guid));
            Assert.IsNull(GameObjectManager.FindGameObject(go, cc2.Guid));

            // Remove the children.
            go.RemoveChildren();
            Assert.IsNull(GameObjectManager.FindGameObject(go, c1.Guid));
        }
    }
}
