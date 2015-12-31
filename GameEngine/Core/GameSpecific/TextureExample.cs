using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    class TextureExample : Scene
    {
        /// <summary>
        /// Vertex array object identifier.
        /// </summary>
        private uint vertexArrayId;

        /// <summary>
        /// Vertex buffer identifier.
        /// </summary>
        private uint vertexbuffer;

        /// <summary>
        /// Vertex buffer data.
        /// </summary>
        private float[] vertexBufferData =
        {
            -1.0f, -1.0f, -5.0f,
             1.0f, -1.0f, -5.0f,
             1.0f,  1.0f, -5.0f,

             1.0f,  1.0f, -5.0f,
            -1.0f,  1.0f, -5.0f,
            -1.0f, -1.0f, -5.0f,
        };

        private float[] uvData = 
        {
            1, 0,
            1, 1,
            0, 1,
            0, 1,
            0, 0,
            1, 0,
        };

        // Storage for the shader programs.
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            // Add default shaders.
            shaders.Add("texture", new ShaderProgram("Core/Shaders/texture-vert.glsl", "Core/Shaders/texture-frag.glsl", true));

            // Load the texture.
            int textureId = LoadTexture(@"C:\development\C#\Textures\Nuclear-Symbol.bmp");

            // Generate a VAO and set it as the current one.
            GL.GenVertexArrays(1, out vertexArrayId);
            GL.BindVertexArray(vertexArrayId);

            // Generate 1 buffer, put the resulting identifier in vertexbuffer.
            GL.GenBuffers(1, out vertexbuffer);

            // The following commands will talk about our 'vertexbuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);

            // Give our vertices to OpenGL.
            IntPtr vertexBufferSize = (IntPtr)(vertexBufferData.Length * Vector3.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertexBufferData, BufferUsageHint.StaticDraw);
        }


        public override void Render()
        {
            // First attribute buffer - vertices
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);

            GL.VertexAttribPointer(
                0,                              // attribute pointer, must match the layout in the shader.
                3,                              // size
                VertexAttribPointerType.Float,  // type
                false,                          // normalized
                0,                              // stride
                0                               // array buffer offset
                );

            // Draw the triangle.
            // Starting from vertex 0; 6 vertices total -> 1 quad.
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);
        }


        /// <summary>
        /// Loads a texture from file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>The generated texture id.</returns>
        private static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            return id;
        }

    }
}
