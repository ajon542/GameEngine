using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    /// <summary>
    /// Quad mesh.
    /// </summary>
    class Quad : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> mesh class.
        /// </summary>
        public Quad()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f),
                new Vector3(1.0f,  1.0f, 0.0f),
                new Vector3(1.0f,  1.0f, 0.0f),
                new Vector3(-1.0f,  1.0f, 0.0f),
                new Vector3(-1.0f, -1.0f, 0.0f),
            };

            Indices = new List<int>
            {
                0, 1, 2, 3, 4, 5
            };

            Normals = new List<Vector3>
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
            };

            Colours = new List<Vector3>
            {
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.9f, 0.9f, 0.9f),
            };

            /// <summary>
            /// UV buffer data.
            /// </summary>
            /// <remarks>
            /// 0,0 --- 1,0
            ///  |       |
            ///  |       |
            /// 0,1 --- 1,1
            /// </remarks>
            UV = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
            };
        }
    }
}
