using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class QuadBehaviour : Behaviour
    {
        private Mesh mesh;

        public override void Initialize()
        {
            mesh = new Mesh();

            mesh.Vertices = new List<Vector3>
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
            mesh.UV = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
            };

            gameObject.AddComponent<Mesh>(mesh);

            gameObject.Transform.Position = new Vector3(0, 0, -5.0f);
        }
    }
}
