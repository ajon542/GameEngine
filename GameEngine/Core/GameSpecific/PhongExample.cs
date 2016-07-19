using OpenTK;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate Phong shading.
    /// </summary>
    public class PhongExample : Scene
    {
        private GameObject gameObject = new GameObject();
        private Renderer renderer;

        public override void Initialize()
        {
            renderer = new Renderer();
            renderer.material = new Material("Core/Shaders/phong43-vert.glsl", "Core/Shaders/phong43-frag.glsl");
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
            Matrix4 modelViewMatrix = gameObject.ModelMatrix * MainCamera.ViewMatrix;
            Matrix4 projectionMatrix = MainCamera.ProjectionMatrix;
            renderer.Render(modelViewMatrix, projectionMatrix);

            // TODO: camera.Render(rootGameObject/scene);
        }

        public override void Shutdown()
        {
            renderer.Destroy();
        }
    }
}
