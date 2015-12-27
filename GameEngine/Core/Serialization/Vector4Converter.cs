using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;

namespace GameEngine.Core.Serialization
{
    /// <summary>
    /// Json converter for Vector objects.
    /// </summary>
    public class Vector4Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector4);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Array)
            {
                var arr = (JArray)obj;
                if (arr.Count == 4 && arr.All(token => token.Type == JTokenType.Float))
                {
                    return new Vector4(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vector = (Vector4)value;
            writer.WriteStartArray();
            writer.WriteValue(vector.X);
            writer.WriteValue(vector.Y);
            writer.WriteValue(vector.Z);
            writer.WriteValue(vector.W);
            writer.WriteEndArray();
        }
    }
}
