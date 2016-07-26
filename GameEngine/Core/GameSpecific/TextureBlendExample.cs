using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

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
            renderer.mesh = new Quad();

            textureId1 = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/UV-Template.bmp");
            textureId2 = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/Nuclear-Symbol.bmp");

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

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId1);
            renderer.material.SetUniform1("textureSampler1", TextureUnit.Texture0 - TextureUnit.Texture0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, textureId2);
            renderer.material.SetUniform1("textureSampler2", TextureUnit.Texture1 - TextureUnit.Texture0);

            renderer.Render(shaderInput);
        }

        public override void Shutdown()
        {
            GL.DeleteTexture(textureId1);
            GL.DeleteTexture(textureId2);
        }
    }
}
