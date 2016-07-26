using OpenTK;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    class TextureExample : Scene
    {
        private GameObject gameObject;
        private Renderer renderer;
        private Light light;

        public override void Initialize()
        {
            gameObject = new GameObject();
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("UnlitTexture", "Core/Shaders/texture-vert.glsl", "Core/Shaders/texture-frag.glsl");
            renderer.material.SetTexture("textureSampler", "Core/GameSpecific/Assets/Textures/UV-Template.bmp");
            renderer.mesh = new Quad();

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
            DefaultShaderInput shaderInput;
            SetDefaultShaderVariables(out shaderInput, gameObject, MainCamera, light);
            renderer.material.UseProgram();
            renderer.Render(shaderInput);
        }

        public override void Shutdown()
        {
            renderer.Destroy();
        }
    }
}
