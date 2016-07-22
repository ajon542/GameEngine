using OpenTK;
using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate Phong shading.
    /// </summary>
    public class PhongExample : Scene
    {
        private GameObject gameObject = new GameObject();
        private Renderer renderer = new Renderer();
        private Light light = new Light(new Vector3(-10, 0, 0), new Vector3());

        public override void Initialize()
        {
            renderer.material = new Material("Core/Shaders/per-pixel-vert.glsl", "Core/Shaders/per-pixel-frag.glsl");
            renderer.mesh = new Sphere(4, 2);

            renderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();
            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            // Construct the default shader input parameters.
            DefaultShaderInput shaderInput = new DefaultShaderInput();
            shaderInput.MatrixMVP = gameObject.ModelMatrix * MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            shaderInput.MatrixMV = gameObject.ModelMatrix * MainCamera.ViewMatrix;
            shaderInput.MatrixV = MainCamera.ViewMatrix;
            shaderInput.MatrixP = MainCamera.ProjectionMatrix;
            shaderInput.MatrixVP = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            shaderInput.Object2World = gameObject.ModelMatrix;
            shaderInput.World2Object = gameObject.ModelMatrix.Inverted();
            shaderInput.WorldCameraPos = MainCamera.Position;
            shaderInput.LightPos = light.Position;

            renderer.Render(shaderInput);

            // TODO: camera.Render(rootGameObject/scene);
        }

        public override void Shutdown()
        {
            renderer.Destroy();
        }
    }
}
