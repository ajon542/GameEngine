using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    class CubeBehaviour : Behaviour
    {
        public Vector3 Colour { get; set; }

        public override void Initialize()
        {
            GameObject.AddComponent<Mesh>(new Cube());

            GameObject.Transform.Position = new Vector3(0, 0, -5.0f);
            GameObject.Transform.Rotation = new Quaternion(1, 1, 1, 0.5f);
        }
    }
}
