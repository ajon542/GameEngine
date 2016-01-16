using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

using GameEngine.Core.Debugging;

namespace GameEngine.Core.GameSpecific
{
    class QuadBehaviour : Behaviour
    {
        public override void Initialize()
        {
            if (GameObject == null)
            {
                // The AddComponent method associates the behaviour with the game object. Before this
                // method is called, the Initialize method can't be called because it will try to do
                // things with the associated game object.
                throw new GameEngineException("game object null");
            }

            GameObject.AddComponent<Mesh>(new Quad());
            GameObject.Transform.Position = new Vector3(0, 0, -3.0f);
        }
    }
}
