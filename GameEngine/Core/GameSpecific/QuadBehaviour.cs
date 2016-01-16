using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class QuadBehaviour : Behaviour
    {
        public override void Initialize()
        {
            GameObject.AddComponent<Mesh>(new Quad());
            GameObject.Transform.Position = new Vector3(0, 0, -3.0f);
        }
    }
}
