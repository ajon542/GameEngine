using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.GameSpecific
{
    // http://www.opengl-tutorial.org/beginners-tutorials/tutorial-2-the-first-triangle/
    class Tutorial2 : Scene
    {
        /// <summary>
        /// Vertex array object identifier.
        /// </summary>
        uint vertexArrayId;

        /// <summary>
        /// Vertex buffer identifier.
        /// </summary>
        uint vertexbuffer;

        /// <summary>
        /// Vertex buffer data.
        /// </summary>
        float[] vertexBufferData =
        {
            -1.0f, -1.0f, -5.0f,
             1.0f, -1.0f, -5.0f,
             0.0f,  1.0f, -5.0f,
        };

        public override void Initialize()
        {
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
                3,                              // size - Vector3
                VertexAttribPointerType.Float,  // type
                false,                          // normalized
                0,                              // stride
                0                               // array buffer offset
                );

            // Draw the triangle.
            // Starting from vertex 0; 3 vertices total -> 1 triangle.
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DisableVertexAttribArray(0);
        }
    }
}
