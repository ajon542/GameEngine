using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core
{
    /// <summary>
    /// The scene class contains all the associated game objects.
    /// </summary>
    /// <remarks>
    /// The scene will contain all the game objects.
    /// The scene will control all the game objects through their behaviour.
    /// The scene will render all game objects in the scene.
    /// </remarks>
    public class Scene
    {
        /// <summary>
        /// Gets or sets the main camera for the scene.
        /// </summary>
        protected Camera MainCamera { get; set; }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Render()
        {
        }

        public virtual void SetupViewPort(int width, int height)
        {
            // Set the view port.
            GL.Viewport(0, 0, width, height);

            if (MainCamera == null)
            {
                MainCamera = new Camera();
            }

            MainCamera.AspectRatio = (width / (float)height);

            // Set the matrix mode and load the matrix.
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projectionMatrix = MainCamera.ProjectionMatrix;
            GL.LoadMatrix(ref projectionMatrix);
        }

        public virtual void Shutdown()
        {
        }

        protected void SetDefaultShaderVariables(out DefaultShaderInput shaderInput, GameObject gameObject, Camera mainCamera, Light light)
        {
            // OpenTK matrices are transposed, hence the multiplication order.
            shaderInput = new DefaultShaderInput();
            shaderInput.MatrixMVP = gameObject.ModelMatrix * MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            shaderInput.MatrixMV = gameObject.ModelMatrix * MainCamera.ViewMatrix;
            shaderInput.MatrixV = MainCamera.ViewMatrix;
            shaderInput.MatrixP = MainCamera.ProjectionMatrix;
            shaderInput.MatrixVP = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            shaderInput.Object2World = gameObject.ModelMatrix;
            shaderInput.World2Object = gameObject.ModelMatrix.Inverted();
            shaderInput.WorldCameraPos = MainCamera.Position;
            shaderInput.LightPosition = light.Position;
            shaderInput.LightColor = light.Color;
        }

        public virtual void AddGameObject(string type)
        {
        }
    }
}
