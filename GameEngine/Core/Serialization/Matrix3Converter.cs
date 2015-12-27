using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;


namespace GameEngine.Core.Serialization
{
    /// <summary>
    /// Json converter for Matrix3 objects.
    /// </summary>
    class Matrix3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Matrix3);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Array)
            {
                var arr = (JArray)obj;
                if (arr.Count == 9 && arr.All(token => token.Type == JTokenType.Float))
                {
                    return new Matrix3(
                        arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(),
                        arr[3].Value<float>(), arr[4].Value<float>(), arr[5].Value<float>(),
                        arr[6].Value<float>(), arr[7].Value<float>(), arr[8].Value<float>()
                        );
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mat = (Matrix3)value;
            writer.WriteStartArray();
            writer.WriteValue(mat.M11);
            writer.WriteValue(mat.M12);
            writer.WriteValue(mat.M13);
            writer.WriteValue(mat.M21);
            writer.WriteValue(mat.M22);
            writer.WriteValue(mat.M23);
            writer.WriteValue(mat.M31);
            writer.WriteValue(mat.M32);
            writer.WriteValue(mat.M33);
            writer.WriteEndArray();
        }
    }
}
