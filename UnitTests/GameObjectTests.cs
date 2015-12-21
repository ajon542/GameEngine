using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
using GameEngine.Core.Serialization;
using OpenTK;

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
    }
}
