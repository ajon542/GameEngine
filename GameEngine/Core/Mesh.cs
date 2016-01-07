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

        // TODO: Normals

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public Mesh()
        {
            Vertices = new List<Vector3>();
            Indices = new List<int>();
            Colours = new List<Vector3>();
            UV = new List<Vector2>(); 
        }

        /// <summary>
        /// Clears the mesh.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
            Colours.Clear();
            UV.Clear();
        }
    }
}
