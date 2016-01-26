using System;
using System.Collections.Generic;
using GameEngine.Core.Debugging;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    // List of geometric vertices, with (x,y,z[,w]) coordinates, w is optional and defaults to 1.0.
    // v 0.123 0.234 0.345 1.0
    class VertexParser : BaseType
    {
        private List<Vector3> vertices;

        public override string Id { get { return "v"; } }

        public VertexParser(List<Vector3> vertices)
        {
            this.vertices = vertices;
        }

        public override void Parse(string input)
        {
            string[] vertexData = input.Split(' ');
            if (vertexData.Length != 3)
            {
                throw new GameEngineException("could not deserialize vertex data");
            }

            vertices.Add(new Vector3
            {
                X = float.Parse(vertexData[0]),
                Y = float.Parse(vertexData[1]),
                Z = float.Parse(vertexData[2]),
            });
        }
    }
}
