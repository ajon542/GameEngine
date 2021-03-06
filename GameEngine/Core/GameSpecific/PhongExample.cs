﻿using OpenTK;
using GameEngine.Core.Graphics;

using NLog;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate Phong shading.
    /// </summary>
    public class PhongExample : Scene
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private GameObject gameObject;
        private Renderer renderer;
        private Light light;

        public override void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            // Create the objects here as the contructor runs quite early on before the GL context has been created.
            // This actually highlights some issues with the intialization procedure. I should be able to use common
            // construction patterns and not be limited to calling these constructors in the Initialize method.
            gameObject = new GameObject();
            renderer = new Renderer();
            light = new Light(new Vector3(10, 0, 0), new Vector4(1, 1, 1, 1));

            renderer.material = new Material("PerPixelMat", "Core/Shaders/per-pixel-vert.glsl", "Core/Shaders/per-pixel-frag.glsl");
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
            // Set the program object for the current rendering state.
            renderer.material.UseProgram();

            // Set the default shader variables.
            DefaultShaderInput shaderInput;
            SetDefaultShaderVariables(out shaderInput, gameObject, MainCamera, light);

            // Set the user specific variables.
            renderer.material.SetVector4("_Color", new Vector4(1, 1, 0, 0));
            renderer.material.SetVector4("_SpecColor", new Vector4(1, 1, 1, 0));
            renderer.material.SetFloat("_Shininess", 100);

            renderer.Render(shaderInput);

            // TODO: camera.Render(rootGameObject/scene);
        }

        public override void Shutdown()
        {
            logger.Log(LogLevel.Info, "");
            renderer.Destroy();
        }
    }
}
