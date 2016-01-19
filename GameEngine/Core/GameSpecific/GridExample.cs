using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class GridExample : Scene
    {
        private GameObject gameObject = new GameObject();

        private ShaderBatch batch;

        public override void Initialize()
        {
            batch = new ShaderBatch(new Grid());
        }

        public override void Update()
        {
            /*var keyboard = Keyboard.GetState();
            if(keyboard[Key.W])
            {
                // TODO: This cam is moving in the z direction when it should be y.
                MainCamera.Move(0f, 0.1f, 0f);
            }
            if (keyboard[Key.S])
            {
                MainCamera.Move(0f, -0.1f, 0f);
            }
            if (keyboard[Key.A])
            {
                MainCamera.Move(-0.1f, 0f, 0f);
            }
            if (keyboard[Key.D])
            {
                MainCamera.Move(0.1f, 0f, 0f);
            }
            if (keyboard[Key.Z])
            {
                MainCamera.Move(0f, 0f, -0.1f);
            }
            if (keyboard[Key.X])
            {
                MainCamera.Move(0f, 0f, 0.1f);
            }

            if (keyboard[Key.R])
            {
                MainCamera.AddRotation(0.1f, 0);
            }
            if (keyboard[Key.T])
            {
                MainCamera.AddRotation(-0.1f, 0f);
            }*/

            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch.Render(gameObject.ModelViewProjectionMatrix);
        }
    }
}
