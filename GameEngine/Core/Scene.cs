﻿using System.Windows.Forms;

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
        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void KeyDown(KeyEventArgs key)
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
