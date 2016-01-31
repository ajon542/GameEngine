using System;
using System.Collections.Generic;
using GameEngine.Core.Debugging;

namespace GameEngine.Core.Utilities.ObjParser
{
    // Polygonal face element (see below)
    // f 1 2 3
    // f 3/1 4/2 5/3
    // f 6/4/1 3/5/3 7/6/5
    class FaceParser : BaseType
    {
        private List<Face> faces;

        protected override string Id { get { return "f"; } }

        // Vertex/TexCoord/Normal Indices
        public FaceParser(List<Face> faces)
        {
            this.faces = faces;
        }

        public override void Parse(string input)
        {
            string[] faceData = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (faceData.Length < 3)
            {
                throw new GameEngineException("not enough vertices to create a face");
            }

            // The faces are defined like a triangle fan around the first vertex.
            // Reconstruct the triangles.
            for (int i = 1; i < faceData.Length - 1; ++i)
            {
                AddFace(faceData[0].Split('/'), faceData[i].Split('/'), faceData[i+1].Split('/'));
            }
        }

        private void AddFace(string[] v0, string[] v1, string[] v2)
        {
            List<int> vertexIndices = new List<int>();
            List<int> uvIndices = new List<int>();
            List<int> normalIndices = new List<int>();

            if (v0.Length == 1 && v1.Length == 1 && v2.Length == 1)
            {
                // Only vertices.
                vertexIndices.Add(Int32.Parse(v0[0]));
                vertexIndices.Add(Int32.Parse(v1[0]));
                vertexIndices.Add(Int32.Parse(v2[0]));
            }
            else if (v0.Length == 2 && v1.Length == 2 && v2.Length == 2)
            {
                // Vertices and texture coords.
                vertexIndices.Add(Int32.Parse(v0[0]));
                vertexIndices.Add(Int32.Parse(v1[0]));
                vertexIndices.Add(Int32.Parse(v2[0]));
            }
            else if (v0.Length == 3 && v1.Length == 3 && v2.Length == 3)
            {
                // Vertices and possible texture coords and normals.
                vertexIndices.Add(Int32.Parse(v0[0]));
                vertexIndices.Add(Int32.Parse(v1[0]));
                vertexIndices.Add(Int32.Parse(v2[0]));

                if (v0[1] != string.Empty)
                {
                    uvIndices.Add(Int32.Parse(v0[1]));
                    uvIndices.Add(Int32.Parse(v1[1]));
                    uvIndices.Add(Int32.Parse(v2[1]));
                }
                if (v0[2] != string.Empty)
                {
                    normalIndices.Add(Int32.Parse(v0[2]));
                    normalIndices.Add(Int32.Parse(v1[2]));
                    normalIndices.Add(Int32.Parse(v2[2]));
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
