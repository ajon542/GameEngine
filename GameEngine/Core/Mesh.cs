using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    public class Mesh : Component
    {
        /// <summary>
        /// Gets or sets how the vertex stream should be interpreted.
        /// </summary>
        public PrimitiveType RenderType { get; set; }

        /// <summary>
        /// Gets or sets the list of vertices.
        /// </summary>
        public List<Vector3> Vertices { get; set; }

        /// <summary>
        /// Gets or sets the list of indices.
        /// </summary>
        public List<int> Indices { get; set; }

        /// <summary>
        /// Gets or sets the list of colours.
        /// </summary>
        public List<Vector3> Colours { get; set; }

        /// <summary>
        /// Gets or sets the list of texture coords.
        /// </summary>
        public List<Vector2> UV { get; set; }

        /// <summary>
        /// Gets or sets the list of normals.
        /// </summary>
        public List<Vector3> Normals { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public Mesh()
        {
            Vertices = new List<Vector3>();
            Indices = new List<int>();
            Colours = new List<Vector3>();
            Normals = new List<Vector3>();
            UV = new List<Vector2>();

            // Set default render primitive.
            RenderType = PrimitiveType.Triangles;
        }

        /// <summary>
        /// Generate the normals for the mesh per face.
        /// </summary>
        /// <remarks>
        /// Clears out current normals.
        /// Since the normals are generated on a per face basis, it will not
        /// give a smooth appearance for curved objects. That may be exactly
        /// what you want, but in most cases, maybe not.
        /// </remarks>
        public void GenerateNormals()
        {
            Normals.Clear();

            for (int i = 2; i < Vertices.Count; i += 3)
            {
                Vector3 v01 = Vertices[i - 1] - Vertices[i - 2];
                Vector3 v12 = Vertices[i] - Vertices[i - 1];

                Vector3 normal = Vector3.Normalize(Vector3.Cross(v01, v12));
                Normals.Add(normal);
                Normals.Add(normal);
                Normals.Add(normal);
            }
        }

        /// <summary>
        /// Generate the texture coords for the mesh.
        /// </summary>
        /// <remarks>
        /// This is a hack and sets every vertex tex coord to (0, 0).
        /// </remarks>
        public void GenerateUVs()
        {
            UV.Clear();

            for (int i = 0; i < Vertices.Count; ++i)
            {
                UV.Add(new Vector2(0, 0));
            }
        }

        /// <summary>
        /// Clears the mesh.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
            Colours.Clear();
            Normals.Clear();
            UV.Clear();
        }
    }
}
