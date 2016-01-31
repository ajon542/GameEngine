using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace GameEngine.Core.Utilities.ObjParser
{
    class ObjFile
    {
        public Mesh Mesh { get; set; }

        private List<Vertex> vertices = new List<Vertex>();
        private List<Normal> normals = new List<Normal>();
        private List<UV> uvs = new List<UV>();
        private List<Face> faces = new List<Face>();
        List<ITypeParser> parsers = new List<ITypeParser>();

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
                        string id = split[0].Trim();
                        string input = split[1].Trim();

                        foreach(ITypeParser parser in parsers)
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
            Mesh = new Mesh();

            foreach (Face face in faces)
            {
                // Vertices.
                int v0 = face.Vertices[0] - 1;
                int v1 = face.Vertices[1] - 1;
                int v2 = face.Vertices[2] - 1;

                Mesh.Vertices.Add(new Vector3(vertices[v0].X, vertices[v0].Y, vertices[v0].Z));
                Mesh.Vertices.Add(new Vector3(vertices[v1].X, vertices[v1].Y, vertices[v1].Z));
                Mesh.Vertices.Add(new Vector3(vertices[v2].X, vertices[v2].Y, vertices[v2].Z));
            }

            // Add the optional normals.
            if(faces[0].Normals.Count > 0)
            {
                foreach (Face face in faces)
                {
                    int n0 = face.Normals[0] - 1;
                    int n1 = face.Normals[1] - 1;
                    int n2 = face.Normals[2] - 1;

                    Mesh.Normals.Add(new Vector3(normals[n0].X, normals[n0].Y, normals[n0].Z));
                    Mesh.Normals.Add(new Vector3(normals[n1].X, normals[n1].Y, normals[n1].Z));
                    Mesh.Normals.Add(new Vector3(normals[n2].X, normals[n2].Y, normals[n2].Z));
                }
            }
            else
            {
                Mesh.GenerateNormals();
            }

            // Add the optional texture coords.
            if (faces[0].UVs.Count > 0)
            {
                foreach (Face face in faces)
                {
                    int u0 = face.UVs[0] - 1;
                    int u1 = face.UVs[1] - 1;
                    int u2 = face.UVs[2] - 1;

                    Mesh.UV.Add(new Vector2(uvs[u0].U, uvs[u0].V));
                    Mesh.UV.Add(new Vector2(uvs[u1].U, uvs[u1].V));
                    Mesh.UV.Add(new Vector2(uvs[u2].U, uvs[u2].V));
                }
            }
            else
            {
                // Generate the colors. This should probably come from the object file too.
                for (int i = 0; i < Mesh.Vertices.Count; ++i)
                {
                    Mesh.Colours.Add(new Vector3(0.9f, 0.9f, 0.9f));
                }
            }

            // Generate the indices.
            for (int i = 0; i < Mesh.Vertices.Count; ++i)
            {
                Mesh.Indices.Add(i);
            }
        }
    }
}
