using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class GridExample : Scene
    {
        private Camera cam = new Camera();
        private GameObject gameObject = new GameObject();

        private ShaderBatch batch;

        public override void Initialize()
        {
            batch = new ShaderBatch(new Grid());
            cam.Move(-50, 0, 0);
        }

        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            if(keyboard[Key.W])
            {
                // TODO: This cam is moving in the z direction when it should be y.
                cam.Move(0f, 0.1f, 0f);
            }
            if (keyboard[Key.S])
            {
                cam.Move(0f, -0.1f, 0f);
            }
            if (keyboard[Key.A])
            {
                cam.Move(-0.1f, 0f, 0f);
            }
            if (keyboard[Key.D])
            {
                cam.Move(0.1f, 0f, 0f);
            }
            if (keyboard[Key.Z])
            {
                cam.Move(0f, 0f, -0.1f);
            }
            if (keyboard[Key.X])
            {
                cam.Move(0f, 0f, 0.1f);
            }

            if (keyboard[Key.R])
            {
                cam.AddRotation(0.1f, 0);
            }
            if (keyboard[Key.T])
            {
                cam.AddRotation(-0.1f, 0f);
            }

            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() * ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch.Render(gameObject.ModelViewProjectionMatrix);
        }
    }
}
