using OpenTK;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class GridExample : Scene
    {
        // TODO: GridExample is broken.
        private GameObject gameObject = new GameObject();

        private Batch batch;

        public override void Initialize()
        {
            batch = new Batch(new Grid());
        }

        public override void Update()
        {
            MainCamera.Update();

            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch.Render();
        }
    }
}
