using System.Collections.Generic;
using OpenTK;

namespace GameEngine.Core
{
    /// <summary>
    /// Skybox mesh.
    /// </summary>
    class SkyBox : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkyBox"/> mesh class.
        /// </summary>
        public SkyBox()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3(1.0f, -1.0f, -1.0f),
                new Vector3(1.0f, -1.0f, -1.0f),
                new Vector3(1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),

                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f,  1.0f),

                new Vector3(1.0f, -1.0f, -1.0f),
                new Vector3(1.0f, -1.0f,  1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(1.0f,  1.0f, -1.0f),
                new Vector3(1.0f, -1.0f, -1.0f),

                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(1.0f, -1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f,  1.0f),

                new Vector3(-1.0f,  1.0f, -1.0f),
                new Vector3(1.0f,  1.0f, -1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),

                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3(1.0f, -1.0f, -1.0f),
                new Vector3(1.0f, -1.0f, -1.0f),
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3(1.0f, -1.0f,  1.0f)
            };

            GenerateNormals();

            Indices = new List<int>();
            Colours = new List<Vector3>();
            for (int i = 0; i < Vertices.Count; ++i)
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
    }
}
