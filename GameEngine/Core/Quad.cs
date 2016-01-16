using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    class Quad : Mesh
    {
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
