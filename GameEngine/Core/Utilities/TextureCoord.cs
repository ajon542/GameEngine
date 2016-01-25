using System;
using GameEngine.Core.Debugging;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    // List of texture coordinates, in (u, v [,w]) coordinates, these will vary between 0 and 1, w is optional and defaults to 0.
    // vt 0.500 1 [0]
    class TextureCoord
    {
        public Vector2 Data { get; set; }

        public TextureCoord(string input)
        {
            Deserialize(input);
        }

        public void Deserialize(string input)
        {
            string[] texData = input.Split(' ');
            if (texData.Length != 2)
            {
                throw new GameEngineException("could not deserialize texture coord data");
            }

            Data = new Vector2
            {
                X = float.Parse(texData[0]),
                Y = float.Parse(texData[1]),
            };
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
