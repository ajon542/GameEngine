using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class LightingExample : Scene
    {
        private GameObject gameObject = new GameObject();
        //private Mesh mesh = new Cube();
        //private Mesh mesh = new Torus(2.0f, 0.5f, 50, 50);
        //private Mesh mesh = new Sphere(6, 2);
        private Mesh mesh = new Mesh();
        Light activeLight = new Light(new Vector3(5, 5, -5), new Vector3(1.0f, 1.0f, 1.0f));

        ShaderBatch batch;

        public override void Initialize()
        {
            ObjFile file = new ObjFile();
            file.Read("Core/GameSpecific/Assets/Mesh/Dragon.obj");
            mesh = file.Mesh;

            batch = new ShaderBatch(mesh);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public float xRot = 0;
        public float yRot = 0;
        public float zRot = 0;
        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            if (keyboard[Key.X])
            {
                xRot += 0.1f;
            }
            if (keyboard[Key.Y])
            {
                yRot += 0.1f;
            }
            if (keyboard[Key.Z])
            {
                zRot += 0.1f;
            }

            MainCamera.Update();

            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.Transform.Rotation = new Quaternion(xRot, yRot, zRot, 1);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch.Render(gameObject.ModelViewProjectionMatrix, MainCamera.ViewMatrix, gameObject.ModelMatrix, activeLight);
        }
    }
}
