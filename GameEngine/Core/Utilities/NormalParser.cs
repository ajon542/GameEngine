using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Core.Debugging;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    // List of vertex normals in (x,y,z) form; normals might not be unit vectors.
    // vn 0.707 0.000 0.707
    class NormalParser : BaseType
    {
        public Vector3 Data { get; set; }

        public override string Id { get { return "vn"; } }

        public NormalParser(string input)
        {
            Parse(input);
        }

        public override void Parse(string input)
        {
            string[] normalData = input.Split(' ');
            if (normalData.Length != 3)
            {
                throw new GameEngineException("could not deserialize vertex data");
            }

            Data = new Vector3
            {
                X = float.Parse(normalData[0]),
                Y = float.Parse(normalData[1]),
                Z = float.Parse(normalData[2]),
            };
        }
    }
}
