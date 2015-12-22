using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    public class Mesh : Component
    {
        /// <summary>
        /// Gets or sets the list of vertices.
        /// </summary>
        public List<Vector3> Vertices { get; set; }

        /// <summary>
        /// Gets or sets the list of triangles.
        /// </summary>
        public List<int> Triangles { get; set; }

        // TODO: UV
        // TODO: Normals

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public Mesh()
        {
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
        }

        /// <summary>
        /// Clears the mesh.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            Triangles.Clear();
        }
    }
}
