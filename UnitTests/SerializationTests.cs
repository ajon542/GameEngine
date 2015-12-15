using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core;
using GameEngine.Core.Serialization;

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

            root.AddChild(c1);

            ISerializer serializer = new Serializer();

            string output = serializer.Serialize(root);
            GameObject loadedRoot = serializer.Deserialize(output);
        }
    }
}
