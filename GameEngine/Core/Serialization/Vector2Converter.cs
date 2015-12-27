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
    public class Vector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Array)
            {
                var arr = (JArray)obj;
                if (arr.Count == 2 && arr.All(token => token.Type == JTokenType.Float))
                {
                    return new Vector2(arr[0].Value<float>(), arr[1].Value<float>());
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vector = (Vector2)value;
            writer.WriteStartArray();
            writer.WriteValue(vector.X);
            writer.WriteValue(vector.Y);
            writer.WriteEndArray();
        }
    }
}
