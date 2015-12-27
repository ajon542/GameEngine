using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core.Serialization;
using OpenTK;

namespace UnitTests
{
    [TestClass]
    public class OpenGLTypeSerializationTests
    {
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
    }
}
