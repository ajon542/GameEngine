using OpenTK;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class GridExample : Scene
    {
        // TODO: GridExample is broken.
        // We are adjusting the camera but we are doing nothing with it.
        private Batch batch;

        public override void Initialize()
        {
            batch = new Batch(new Grid());
        }

        public override void Update()
        {
            MainCamera.Update();
        }

        public override void Render()
        {
            batch.Render();
        }
    }
}
