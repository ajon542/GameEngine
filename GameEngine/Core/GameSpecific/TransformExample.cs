using OpenTK;
using OpenTK.Input;
using GameEngine.Core.Graphics;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// This example is to test different aspects of game object transformation
    /// with respect to the parent game object.
    /// </summary>
    class TransformExample : Scene
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private GameObject gameObject;
        private GameObject child1;
        private GameObject child2;
        private GameObject child3;
        private Renderer renderer;
        private Light light;

        public override void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            // Create the game objects.
            gameObject = new GameObject();
            child1 = new GameObject();
            child2 = new GameObject();
            child3 = new GameObject();

            // Attach child object.
            gameObject.AddChild(child1);
            child1.AddChild(child2);
            child2.AddChild(child3);

            // The parent object is essentially position (0, 0, 0) for the child objects.
            // child1 is at position (0, 0, -10) in world coordinates.
            // child2 is at position (0, 0, -20) in world coordinates.
            gameObject.Transform.Position = new Vector3(0, 0, 0);
            child1.Transform.Position = new Vector3(0, 0, -20);
            child2.Transform.Position = new Vector3(0, 0, -10);
            child3.Transform.Position = new Vector3(0, 0, -5);

            // Use the same renderer and light for all game objects.
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("PerPixelMat", "Core/Shaders/per-pixel-vert.glsl", "Core/Shaders/per-pixel-frag.glsl");
            renderer.mesh = new Torus(1, 0.3f, 20, 20);

            renderer.Initialize();
        }

        private float position;
        private float rotation1;
        private float rotation2;
        private float rotation3;
        public override void Update()
        {
            MainCamera.Update();

            var keyboard = Keyboard.GetState();
            if (keyboard[Key.P])
            {
                position -= 0.1f;
            }
            if (keyboard[Key.Keypad1])
            {
                rotation1 += 0.1f;
            }
            if (keyboard[Key.Keypad2])
            {
                rotation2 += 0.1f;
            }
            if (keyboard[Key.Keypad3])
            {
                rotation3 += 0.1f;
            }

            gameObject.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, rotation1);
            gameObject.Transform.Position = new Vector3(0, 0, position);

            child1.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, rotation2);
            child2.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, rotation3);

            gameObject.CalculateModelMatrix();
            child1.CalculateModelMatrix();
            child2.CalculateModelMatrix();
            child3.CalculateModelMatrix();
        }

        public override void Render()
        {
            // Set the program object for the current rendering state.
            renderer.material.UseProgram();

            // Set the default shader variables.
            DefaultShaderInput shaderInput;
            SetDefaultShaderVariables(out shaderInput, gameObject, MainCamera, light);

            // Set the user specific variables.
            renderer.material.SetVector4("_Color", new Vector4(1, 1, 0, 0));
            renderer.material.SetVector4("_SpecColor", new Vector4(1, 1, 1, 0));
            renderer.material.SetFloat("_Shininess", 100);

            // Render the parent game object.
            renderer.Render(shaderInput);

            // Calculate the new model matrix for the child game object.
            child1.ModelMatrix = child1.ModelMatrix * gameObject.ModelMatrix;
            SetDefaultShaderVariables(out shaderInput, child1, MainCamera, light);
            renderer.Render(shaderInput);

            // Calculate the new model matrix for the child game object.
            child2.ModelMatrix = child2.ModelMatrix * child1.ModelMatrix;
            SetDefaultShaderVariables(out shaderInput, child2, MainCamera, light);
            renderer.Render(shaderInput);

            // Calculate the new model matrix for the child game object.
            child3.ModelMatrix = child3.ModelMatrix * child2.ModelMatrix;
            SetDefaultShaderVariables(out shaderInput, child3, MainCamera, light);
            renderer.Render(shaderInput);
        }

        public override void Shutdown()
        {
            logger.Log(LogLevel.Info, "");
            renderer.Destroy();
        }
    }
}
