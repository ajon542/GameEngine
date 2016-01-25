using System;
using GameEngine.Core.Debugging;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    // List of geometric vertices, with (x,y,z[,w]) coordinates, w is optional and defaults to 1.0.
    // v 0.123 0.234 0.345 1.0
    class Vertex
    {
        public Vector3 Data { get; set; }

        public Vertex(string input)
        {
            Deserialize(input);
        }

        public void Deserialize(string input)
        {
            string[] vertexData = input.Split(' ');
            if (vertexData.Length != 3)
            {
                throw new GameEngineException("could not deserialize vertex data");
            }

            Data = new Vector3
            {
                X = float.Parse(vertexData[0]),
                Y = float.Parse(vertexData[1]),
                Z = float.Parse(vertexData[2]),
            };
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
