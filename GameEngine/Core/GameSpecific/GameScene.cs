using System;

using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ShaderBatch batch1;
        private ShaderBatch batch2;

        private int mouseWheelIndex = 0;

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        public override void Initialize()
        {
            // Set up depth test and face culling.
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Add our cube behaviour.
            gameObject.AddComponent<Behaviour>(new CubeBehaviour());
            gameObject.GetComponent<Behaviour>().Initialize();

            gameObject2.AddComponent<Behaviour>(new CubeBehaviour());
            gameObject2.GetComponent<Behaviour>().Initialize();

            batch1 = new ShaderBatch(gameObject.GetComponent<Mesh>());
            batch2 = new ShaderBatch(gameObject2.GetComponent<Mesh>());
        }

        public override void Update()
        {
            // TODO: A little inefficient
            gameObject.GetComponent<Behaviour>().Update();
            gameObject2.GetComponent<Behaviour>().Update();

            // Handle zoom.
            var mouse = Mouse.GetState();
            if (mouseWheelIndex != mouse.Wheel)
            {
                Vector3 vec = MainCamera.LookAt - MainCamera.Position;

                if (mouseWheelIndex > mouse.Wheel)
                {
                    vec *= 0.5f;
                }
                else
                {
                    vec *= -0.5f;
                }
                
                MainCamera.LookAt = new Vector3(
                    MainCamera.LookAt.X + vec.X,
                    MainCamera.LookAt.Y + vec.Y,
                    MainCamera.LookAt.Z + vec.Z
                    );
                MainCamera.Position = new Vector3(
                    MainCamera.Position.X + vec.X,
                    MainCamera.Position.Y + vec.Y,
                    MainCamera.Position.Z + vec.Z
                    );
                mouseWheelIndex = mouse.Wheel;
                Console.WriteLine("LookAt {0}, Position {1}, Vec {2}", MainCamera.LookAt, MainCamera.Position, vec);
            }


            // Update.
            gameObject.Transform.Position = new Vector3(-1, 0, -5.0f);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

            gameObject2.Transform.Position = new Vector3(1, 0, -5.0f);
            gameObject2.CalculateModelMatrix();
            gameObject2.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject2.ModelViewProjectionMatrix = gameObject2.ModelMatrix * gameObject2.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch1.Render(gameObject.ModelViewProjectionMatrix);
            batch2.Render(gameObject2.ModelViewProjectionMatrix);
        }
    }
}
