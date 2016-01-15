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

        // NOTES:
        // Thinking about this from a top-down approach, it we want to have a single
        // call such as gameObject.AddComponent<Cube>() and then have that cube rendered
        // in the scene.
        //
        // Cube will have a mesh containing verts, colours, indices, uvs.
        // Initialize the scene and use the corresponding shader.
        // Render the scene accordingly.
        // Consider having a single shader for each mesh / game object.
        // Submit the mesh and specify the shader we are going to use with it.
        // For simplicities sake, we can have two shaders:
        // 1. Vert and colors
        // 2. Texture

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Render()
        {
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
