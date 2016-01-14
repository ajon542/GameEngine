using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class VaoExample : Scene
    {
        private GameObject gameObject = new GameObject();

        private Batch batch1;
        private Batch batch2;

        public override void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Add our cube behaviour.
            gameObject.AddComponent<Behaviour>(new CubeBehaviour { Colour = new Vector3(1, 0, 0) });
            gameObject.GetComponent<CubeBehaviour>().Initialize();

            batch1 = new Batch(gameObject.GetComponent<Mesh>());
            batch2 = new Batch(gameObject.GetComponent<Mesh>());
        }

        public override void Render()
        {
            // TODO: Remove this pushmatrix stuff. We should be passing these vertices into a shader
            // along with the mvp matrix.
            // http://sol.gfxile.net/instancing.html
            GL.PushMatrix();
            GL.Translate(0, 0, -3);
            batch1.Render();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(2, 0, -3);
            batch2.Render();
            GL.PopMatrix();
        }
    }
}