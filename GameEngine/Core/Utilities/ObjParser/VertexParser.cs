using System.Collections.Generic;
using GameEngine.Core.Debugging;

namespace GameEngine.Core.Utilities.ObjParser
{
    // List of geometric vertices, with (x,y,z[,w]) coordinates, w is optional and defaults to 1.0.
    // v 0.123 0.234 0.345 1.0
    class VertexParser : BaseType
    {
        private List<Vertex> vertices;

        protected override string Id { get { return "v"; } }

        public VertexParser(List<Vertex> vertices)
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

            vertices.Add(new Vertex
            {
                X = float.Parse(vertexData[0]),
                Y = float.Parse(vertexData[1]),
                Z = float.Parse(vertexData[2]),
            });
        }
    }
}
