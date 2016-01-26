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

        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();
        private List<Face> faces = new List<Face>();
        List<IObjType> parsers = new List<IObjType>();

        public ObjFile()
        {
            parsers.Add(new VertexParser(vertices));
            parsers.Add(new NormalParser(normals));
            parsers.Add(new UVParser(uvs));
            parsers.Add(new FaceParser(faces));
        }

        public void Read(string file)
        {
            // Read the file into the appropriate data structures.
            string line;
            using (StreamReader sr = new StreamReader(file))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] split = line.Split(new[] {' '}, 2);
                    
                    if (split.Length > 1)
                    {
                        string id = split[0];
                        string input = split[1];

                        foreach(IObjType parser in parsers)
                        {
                            if(parser.CanParse(id))
                            {
                                parser.Parse(input);
                            }
                        }
                    }
                }
            }

            // Map the data into the Vertices, Normals, TexCoords and Indices data structures.
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            TexCoords = new List<Vector2>();
            Indices = new List<int>();
            foreach (Face face in faces)
            {
                // Vertices.
                int v0 = face.Vertices[0] - 1;
                int v1 = face.Vertices[1] - 1;
                int v2 = face.Vertices[2] - 1;

                Vertices.Add(vertices[v0]);
                Vertices.Add(vertices[v1]);
                Vertices.Add(vertices[v2]);

                // Normals.
                int n0 = face.Normals[0] - 1;
                int n1 = face.Normals[1] - 1;
                int n2 = face.Normals[2] - 1;

                Normals.Add(normals[n0]);
                Normals.Add(normals[n1]);
                Normals.Add(normals[n2]);

                // Texture Coords.
                int t0 = face.UVs[0] - 1;
                int t1 = face.UVs[1] - 1;
                int t2 = face.UVs[2] - 1;

                TexCoords.Add(uvs[t0]);
                TexCoords.Add(uvs[t1]);
                TexCoords.Add(uvs[t2]);
            }

            int index = 0;
            foreach (Vector3 vector in Vertices)
            {
                Indices.Add(index++);
            }
        }
    }
}
