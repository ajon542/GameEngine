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
        private Mesh mesh = new Quad();
        Light activeLight = new Light(new Vector3(0, -5, 0), new Vector3(1.0f, 1.0f, 1.0f));

        ShaderBatch batch;

        public override void Initialize()
        {
            MainCamera.Position = new Vector3(0, 10, 100);

            batch = new ShaderBatch(mesh);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public override void Update()
        {
            MainCamera.Update();

            gameObject.Transform.Position = new Vector3(0, -10, 0);
            gameObject.Transform.Scale = new Vector3(40, 40, 1);
            gameObject.Transform.Rotation = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), DegreesToRadians(-90));
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            batch.Render(gameObject.ModelViewProjectionMatrix, MainCamera.ViewMatrix, gameObject.ModelMatrix, activeLight);
        }

        // TODO: Move into a helper class.
        private float DegreesToRadians(float degrees)
        {
            return (degrees * (float)Math.PI) / 180.0f;
        }
    }
}
