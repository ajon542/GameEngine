using System;
using GameEngine.Core.Debugging;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    // List of texture coordinates, in (u, v [,w]) coordinates, these will vary between 0 and 1, w is optional and defaults to 0.
    // vt 0.500 1 [0]
    // TODO: Support the optional w
    class UVParser : BaseType
    {
        public Vector2 Data { get; set; }

        public override string Id { get { return "vt"; } }

        public UVParser(string input)
        {
            Parse(input);
        }

        public override void Parse(string input)
        {
            string[] texData = input.Split(' ');
            //if (texData.Length != 2)
            if (texData.Length < 2)
            {
                throw new GameEngineException("could not deserialize texture coord data");
            }

            Data = new Vector2
            {
                X = float.Parse(texData[0]),
                Y = float.Parse(texData[1]),
            };
        }
    }
}
