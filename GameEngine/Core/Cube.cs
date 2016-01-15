using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    /// <summary>
    /// Cube mesh.
    /// </summary>
    class Cube : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cube"/> mesh class.
        /// </summary>
        public Cube()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

            Indices = new List<int>
            {
                0, 2, 1, 0, 3, 2, // left
                1, 2, 6, 6, 5, 1, // back
                4, 5, 6, 6, 7, 4, // right
                2, 3, 6, 6, 3, 7, // top
                0, 7, 3, 0, 4, 7, // front
                0, 1, 5, 0, 5, 4  // bottom
            };

            Colours = new List<Vector3>
            {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
            };
        }
    }
}
