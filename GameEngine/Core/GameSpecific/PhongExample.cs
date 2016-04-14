using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example to demonstrate Phong shading.
    /// </summary>
    public class PhongExample : Scene
    {
        private GameObject gameObject = new GameObject();
        private Mesh mesh = new Sphere(4, 2);
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        private Renderer renderer;

        public override void Shutdown()
        {
            shaders = new Dictionary<string, ShaderProgram>();
        }

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/phong43-vert.glsl", "Core/Shaders/phong43-frag.glsl", true));

            Material material = new Material();
            material.shaderProgram = shaders["default"];

            renderer = new Renderer();
            renderer.material = material;
            renderer.mesh = mesh;

            renderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();
            // TODO: I could almost argue that these belong in the Renderer class.
            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            // TODO: Since this is the game object renderer there should be no need to pass in the matrices...
            Matrix4 modelViewMatrix = gameObject.ModelMatrix * MainCamera.ViewMatrix;
            Matrix4 projectionMatrix = MainCamera.ProjectionMatrix;
            renderer.Render(modelViewMatrix, projectionMatrix);
        }
    }
}
