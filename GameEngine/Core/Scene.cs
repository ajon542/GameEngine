using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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

            MainCamera = new Camera();
            MainCamera.AspectRatio = (width / (float)height);
            MainCamera.FieldOfView = 1.0f;
            MainCamera.NearPlane = 1.0f;
            MainCamera.FarPlane = 1000.0f;

            // Set the matrix mode and load the matrix.
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projectionMatrix = MainCamera.ProjectionMatrix;
            GL.LoadMatrix(ref projectionMatrix);
        }

        public virtual void Shutdown()
        {
            // TODO: Cleanup / shutdown
            //glDeleteBuffers(1, &vertexbuffer);
            //glDeleteBuffers(1, &uvbuffer);
            //glDeleteProgram(programID);
            //glDeleteTextures(1, &TextureID);
            //glDeleteVertexArrays(1, &VertexArrayID);
        }
    }
}
