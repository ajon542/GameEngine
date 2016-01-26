using System;
using System.Collections.Generic;
using GameEngine.Core.Debugging;

namespace GameEngine.Core.Utilities
{
    // TODO: Support face data greater than 3

    // Polygonal face element (see below)
    // f 1 2 3
    // f 3/1 4/2 5/3
    // f 6/4/1 3/5/3 7/6/5
    class FaceParser : BaseType
    {
        private List<Face> faces;

        public override string Id { get { return "f"; } }

        // Vertex/TexCoord/Normal Indices
        public FaceParser(List<Face> faces)
        {
            this.faces = faces;
        }

        public override void Parse(string input)
        {
            string[] faceData = input.Split(' ');
            if (faceData.Length != 3)
            {
                throw new GameEngineException("could not deserialize face data");
            }

            List<int> vertexIndices = new List<int>();
            List<int> uvIndices = new List<int>();
            List<int> normalIndices = new List<int>();

            foreach (string item in faceData)
            {
                string[] split = item.Split('/');

                if (split.Length == 1)
                {
                    // Only contains vertex indices.
                    vertexIndices.Add(Int32.Parse(split[0]));
                }
                else if (split.Length == 3)
                {
                    vertexIndices.Add(Int32.Parse(split[0]));
                    if (split[1] != string.Empty)
                    {
                        uvIndices.Add(Int32.Parse(split[1]));
                    }
                    if (split[2] != string.Empty)
                    {
                        normalIndices.Add(Int32.Parse(split[2]));
                    }
                }
                else
                {
                    throw new GameEngineException("could not deserialize face data");
                }

                Face face = new Face();
                face.Vertices = vertexIndices;
                face.Normals = normalIndices;
                face.UVs = uvIndices;

                faces.Add(face);
            }
        }
    }
}
