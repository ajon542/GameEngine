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
                // Back
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),

                // Right
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),

                // Front
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

                // Left
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

            AddFaceNormals(new Vector3(0, 0, -1)); // Back
            AddFaceNormals(new Vector3(1, 0, 0));  // Right
            AddFaceNormals(new Vector3(0, 0, 1));  // Front
            AddFaceNormals(new Vector3(0, 1, 0));  // Top
            AddFaceNormals(new Vector3(-1, 0, 0)); // Left
            AddFaceNormals(new Vector3(0, -1, 0)); // Bottom

            Indices = new List<int>();
            Colours = new List<Vector3>();
            for(int i = 0; i < Vertices.Count; ++i)
            {
                Indices.Add(i);
                Colours.Add(new Vector3(1, 1, 1));
            }

            UV = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
            };
        }

        /// <summary>
        /// Helper method to add the face normals.
        /// </summary>
        /// <param name="normal">The normal vector.</param>
        private void AddFaceNormals(Vector3 normal)
        {
            for(int i = 0; i < 6; ++i)
            {
                Normals.Add(normal);
            }
        }
    }
}
