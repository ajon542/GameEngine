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
                // Left
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),

                // Back
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),

                // Right
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),

                // Top
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),

                // Front
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),

                // Bottom
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
            };

            Indices = new List<int>
            {
                0, 1, 2, 3, 4, 5,
                6, 7, 8, 9, 10, 11,
                12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34, 35
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
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
            };

            Normals = new List<Vector3>
            {
                // Left
                new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(-1, 0, 0),

                // Back
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),

                // Right
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),

                // Top
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),

                // Front
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),

                // Bottom
                new Vector3(0, -1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, -1, 0),
            };
        }
    }
}
