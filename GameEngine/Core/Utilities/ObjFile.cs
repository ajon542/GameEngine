using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace GameEngine.Core.Utilities
{
    class ObjFile
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector2> TexCoords { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<int> Indices { get; set; }

        private List<VertexParser> vertices = new List<VertexParser>();
        private List<NormalParser> normals = new List<NormalParser>();
        private List<UVParser> texCoords = new List<UVParser>();
        private List<FaceParser> faces = new List<FaceParser>();

        public void Read(string file)
        {
            // Read the file into the appropriate data structures.
            string line;
            using (StreamReader sr = new StreamReader(file))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] split = line.Split(new[] {' '}, 2);

                    if (split.Length > 0)
                    {
                        if (split[0] == "v")
                        {
                            VertexParser type = new VertexParser(split[1]);
                            vertices.Add(type);
                        }
                        else if (split[0] == "vt")
                        {
                            UVParser type = new UVParser(split[1]);
                            texCoords.Add(type);
                        }
                        else if (split[0] == "vn")
                        {
                            NormalParser type = new NormalParser(split[1]);
                            normals.Add(type);
                        }
                        else if (split[0] == "f")
                        {
                            FaceParser type = new FaceParser(split[1]);
                            faces.Add(type);
                        }
                    }
                }
            }

            // Map the data into the Vertices, Normals, TexCoords and Indices data structures.
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            TexCoords = new List<Vector2>();
            Indices = new List<int>();
            foreach (FaceParser face in faces)
            {
                // Vertices.
                int v0 = face.VertexIndices[0] - 1;
                int v1 = face.VertexIndices[1] - 1;
                int v2 = face.VertexIndices[2] - 1;

                Vertices.Add(vertices[v0].Data);
                Vertices.Add(vertices[v1].Data);
                Vertices.Add(vertices[v2].Data);

                // Normals.
                int n0 = face.NormalIndices[0] - 1;
                int n1 = face.NormalIndices[1] - 1;
                int n2 = face.NormalIndices[2] - 1;

                Normals.Add(normals[n0].Data);
                Normals.Add(normals[n1].Data);
                Normals.Add(normals[n2].Data);

                // Texture Coords.
                int t0 = face.TexCoordIndices[0] - 1;
                int t1 = face.TexCoordIndices[1] - 1;
                int t2 = face.TexCoordIndices[2] - 1;

                TexCoords.Add(texCoords[t0].Data);
                TexCoords.Add(texCoords[t1].Data);
                TexCoords.Add(texCoords[t2].Data);
            }

            int index = 0;
            foreach (Vector3 vector in Vertices)
            {
                Indices.Add(index++);
            }
        }
    }
}
