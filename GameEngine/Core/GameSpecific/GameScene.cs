using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example of creating a game specific scene.
    /// </summary>
    class GameScene : Scene
    {
        private GameObject gameObject = new GameObject();
        private GameObject gameObject2 = new GameObject();

        private Camera cam = new Camera();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ShaderBatch batch1;
        private ShaderBatch batch2;

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        public override void Initialize()
        {
            // Set up depth test and face culling.
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Add our cube behaviour.
            CubeBehaviour cubeBehaviour = new CubeBehaviour();
            cubeBehaviour.Colour = new Vector3(1, 0, 0);
            gameObject.AddComponent<Behaviour>(cubeBehaviour);
            cubeBehaviour.Initialize();

            CubeBehaviour cubeBehaviour2 = new CubeBehaviour();
            cubeBehaviour2.Colour = new Vector3(0, 0, 1);
            gameObject2.AddComponent<Behaviour>(cubeBehaviour2);
            cubeBehaviour2.Initialize();

            batch1 = new ShaderBatch(gameObject.GetComponent<Mesh>());
            batch2 = new ShaderBatch(gameObject2.GetComponent<Mesh>());
        }

        public override void Update()
        {
            // TODO: A little inefficient
            Behaviour component = gameObject.GetComponent<Behaviour>();

            if (component != null)
            {
                component.Update();
            }

            component = gameObject2.GetComponent<Behaviour>();

            if (component != null)
            {
                component.Update();
            }

            // Update...
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.ViewMatrix * ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

            gameObject2.Transform.Position = new Vector3(2, 0, -5.0f);
            gameObject2.CalculateModelMatrix();
            gameObject2.ViewProjectionMatrix = cam.ViewMatrix * ProjectionMatrix;
            gameObject2.ModelViewProjectionMatrix = gameObject2.ModelMatrix * gameObject2.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch1.Render(gameObject.ModelViewProjectionMatrix);
            batch2.Render(gameObject2.ModelViewProjectionMatrix);
        }
    }
}
