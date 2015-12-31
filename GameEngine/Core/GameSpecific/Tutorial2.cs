using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.GameSpecific
{
    class Tutorial2 : Scene
    {
        // This will identify our vertex buffer
        uint vertexbuffer;
        uint VertexArrayID;
        float[] g_vertex_buffer_data = new float[]{
            -1.0f, -1.0f, -3.0f,
            1.0f, -1.0f, -3.0f,
            0.0f,  1.0f, -3.0f,
            };

        public override void Initialize()
        {
            // Other state
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0, 0.1f, 0.4f, 1);

            GL.GenVertexArrays(1, out VertexArrayID);
            GL.BindVertexArray(VertexArrayID);

            // Generate 1 buffer, put the resulting identifier in vertexbuffer
            GL.GenBuffers(1, out vertexbuffer);
            // The following commands will talk about our 'vertexbuffer' buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            // Give our vertices to OpenGL.
            IntPtr vertexBufferSize = (IntPtr)(g_vertex_buffer_data.Length * Vector3.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, g_vertex_buffer_data, BufferUsageHint.StaticDraw);
        }


        public override void Render()
        {
            // 1rst attribute buffer : vertices
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(
                0,                              // attribute 0. No particular reason for 0, but must match the layout in the shader.
                3,                              // size
                VertexAttribPointerType.Float,  // type
                false,                          // normalized?
                0,                              // stride
                0                               // array buffer offset
                );
            // Draw the triangle !
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // Starting from vertex 0; 3 vertices total -> 1 triangle 
            GL.DisableVertexAttribArray(0);

            GL.Flush();
        }
    }
}
