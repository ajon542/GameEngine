using System.Collections.Generic;
using System.IO;

namespace GameEngine.Core.Utilities
{
    class ObjFile
    {
        private List<Vertex> vertices = new List<Vertex>();
        private List<Normal> normals = new List<Normal>();
        private List<TextureCoord> texCoords = new List<TextureCoord>();
        private List<Face> faces = new List<Face>();

        public void Read(string file)
        {
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
                            Vertex type = new Vertex(split[1]);
                            vertices.Add(type);
                        }
                        else if (split[0] == "vt")
                        {
                            TextureCoord type = new TextureCoord(split[1]);
                            texCoords.Add(type);
                        }
                        else if (split[0] == "vn")
                        {
                            Normal type = new Normal(split[1]);
                            normals.Add(type);
                        }
                        else if (split[0] == "f")
                        {
                            Face type = new Face(split[1]);
                            faces.Add(type);
                        }
                    }
                }
            }
        }
    }
}
