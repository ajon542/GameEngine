using OpenTK;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class TextureBlendExample : Scene
    {
        private GameObject gameObject;
        private Renderer renderer;
        private Light light;

        private int textureId1;
        private int textureId2;

        public override void Initialize()
        {
            gameObject = new GameObject();
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("TextureBlend", "Core/Shaders/texture-blend-vert.glsl", "Core/Shaders/texture-blend-frag.glsl");
            renderer.material.SetTexture("textureSampler1", "Core/GameSpecific/Assets/Textures/UV-Template.bmp");
            renderer.material.SetTexture("textureSampler2", "Core/GameSpecific/Assets/Textures/Nuclear-Symbol.bmp");
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
