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

        [TestMethod]
        public void TestMatrix3Serialization()
        {
            Matrix3 m1 = new Matrix3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            ISerializer<Matrix3> serializer = new Serializer<Matrix3>();
            string output = serializer.Serialize(m1);
            Matrix3 res = serializer.Deserialize(output);
            Assert.AreEqual(m1, res);

            Matrix3 m2 = new Matrix3(new Vector3(1, 4, 7), new Vector3(2, 5, 8), new Vector3(3, 6, 9));
            output = serializer.Serialize(m2);
            res = serializer.Deserialize(output);
            Assert.AreEqual(m2, res);
        }

        [TestMethod]
        public void TestMatrix4Serialization()
        {
            Matrix4 m1 = new Matrix4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            ISerializer<Matrix4> serializer = new Serializer<Matrix4>();
            string output = serializer.Serialize(m1);
            Matrix4 res = serializer.Deserialize(output);
            Assert.AreEqual(m1, res);

            Matrix4 m2 = new Matrix4(new Vector4(1, 2, 3, 4), new Vector4(5, 6, 7, 8), new Vector4(9, 10, 11, 12), new Vector4(13, 14, 15, 16));
            output = serializer.Serialize(m2);
            res = serializer.Deserialize(output);
            Assert.AreEqual(m2, res);
        }

        [TestMethod]
        public void TestBox2Serialization()
        {
            Box2 b = new Box2(1, 2, 3, 4);
            ISerializer<Box2> serializer = new Serializer<Box2>();
            string output = serializer.Serialize(b);
            Box2 res = serializer.Deserialize(output);
            Assert.AreEqual(b, res);
        }
    }
}
