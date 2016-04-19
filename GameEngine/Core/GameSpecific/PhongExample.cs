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
        private Renderer renderer;

        public override void Initialize()
        {
            Material material = new Material("Core/Shaders/phong43-vert.glsl", "Core/Shaders/phong43-frag.glsl");

            renderer = new Renderer();
            renderer.material = material;
            renderer.mesh = new Sphere(4, 2);

            renderer.Initialize();
        }

        public override void Update()
        {
            MainCamera.Update();
            // TODO: I could almost argue that these belong in the Renderer class.
            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            // TODO: Since this is the game object renderer there should be no need to pass in the matrices...
            Matrix4 modelViewMatrix = gameObject.ModelMatrix * MainCamera.ViewMatrix;
            Matrix4 projectionMatrix = MainCamera.ProjectionMatrix;
            renderer.Render(modelViewMatrix, projectionMatrix);
        }

        public override void Shutdown()
        {
            // TODO: Handle this cleanup
            //shaders = new Dictionary<string, ShaderProgram>();
        }
    }
}
