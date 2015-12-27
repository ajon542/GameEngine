using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameEngine.Core.Serialization;

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
            Base b = new Derived { Value = 1234, Name = "Derived" };
            ISerializer<Base> serializer = new Serializer<Base>();
            string output = serializer.Serialize(b);
            Base res = serializer.Deserialize(output);
        }
    }
}
