using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class CubeBehaviour : Behaviour
    {
        public override void Initialize()
        {
            GameObject.AddComponent<Mesh>(new Cube());
            GameObject.Transform.Position = new Vector3(0, 0, -5.0f);
        }
    }
}
