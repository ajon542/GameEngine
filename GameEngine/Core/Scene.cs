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
        public Matrix4 ProjectionMatrix { get; set; }

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

            // Create the perspective field of view matrix.
            float aspectRatio = (width / (float)height);
            float fov = 1f;
            float near = 1.0f;
            float far = 1000.0f;

            Matrix4 perspectiveMatrix =
               Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, near, far);

            ProjectionMatrix = perspectiveMatrix;

            // Set the matrix mode and load the matrix.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMatrix);
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
