using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

using GameEngine.Core.Debugging;

namespace GameEngine.Core.GameSpecific
{
    class QuadBehaviour : Behaviour
    {
        private Mesh mesh;

        public override void Initialize()
        {
            if(gameObject == null)
            {
                // The AddComponent method associates the behaviour with the game object. Before this
                // method is called, the Initialize method can't be called because it will try to do
                // things with the associated game object.
                throw new GameEngineException("game object null");
            }

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

            gameObject.Transform.Position = new Vector3(0, 0, -3.0f);
        }
    }
}
