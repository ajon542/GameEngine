using System.Collections.Generic;

using OpenTK;
using OpenTK.Input;
using GameEngine.Core.Graphics;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// This example demostrates the ability to add a game object
    /// through the UI.
    /// </summary>
    class GameObjectUIExample : Scene
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private GameObject root;
        private Renderer renderer;
        private Light light;

        public override void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            // Create the game objects.
            root = new GameObject();

            // Use the same renderer and light for all game objects.
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("PerPixelMat", "Core/Shaders/per-pixel-vert.glsl", "Core/Shaders/per-pixel-frag.glsl");
            renderer.mesh = new Torus(1, 0.3f, 20, 20);

            renderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();

            List<GameObject> children = root.GetChildren();
            foreach (GameObject go in children)
            {
                go.CalculateModelMatrix();
            }
        }

        public override void Render()
        {
            // Set the program object for the current rendering state.
            renderer.material.UseProgram();

            // Set the user specific variables.
            renderer.material.SetVector4("_Color", new Vector4(1, 1, 0, 0));
            renderer.material.SetVector4("_SpecColor", new Vector4(1, 1, 1, 0));
            renderer.material.SetFloat("_Shininess", 100);

            List<GameObject> children = root.GetChildren();
            foreach (GameObject go in children)
            {
                // Set the default shader variables.
                DefaultShaderInput shaderInput;
                SetDefaultShaderVariables(out shaderInput, go, MainCamera, light);

                // Render the game object.
                renderer.Render(shaderInput);
            }
        }

        public override void Shutdown()
        {
            logger.Log(LogLevel.Info, "");
            renderer.Destroy();
        }

        private float zPosition = 0;
        public override void AddGameObject(string type)
        {
            GameObject go = new GameObject();
            go.Transform.Position = new Vector3(0, 0, zPosition);
            zPosition -= 10;

            root.AddChild(go);
        }
    }
}
