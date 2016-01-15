using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// An example to investigate the performance enhancements acheived from instancing.
    /// </summary>
    /// <remarks>
    /// TODO: Complete the instancing example.
    /// http://ogldev.atspace.co.uk/www/tutorial33/tutorial33.html
    /// http://sol.gfxile.net/instancing.html
    /// </remarks>
    public class InstancingExample : Scene
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

        private GameObject gameObject = new GameObject();
        private Camera cam = new Camera();
        private List<Matrix4> transformMatrices = new List<Matrix4>();

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

            GL.VertexAttribPointer(
                0,                              // attribute pointer, must match the layout in the shader.
                3,                              // size - Vector3
                VertexAttribPointerType.Float,  // type
                false,                          // normalized
                0,                              // stride
                0                               // array buffer offset
                );

            int count = 300;
            // Initialize the matrices.
            for (int x = -count; x < count; x++)
            {
                for (int y = -count; y < count; y++)
                {
                    gameObject.Transform.Position = new Vector3(x, y, -600);
                    gameObject.CalculateModelMatrix();
                    gameObject.ViewProjectionMatrix = cam.GetViewMatrix() * ViewProjectionMatrix;
                    gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

                    transformMatrices.Add(gameObject.ModelViewProjectionMatrix);
                }
            }
        }


        public override void Render()
        {
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);

            foreach (Matrix4 matrix in transformMatrices)
            {
                Matrix4 m = matrix;
                GL.LoadMatrix(ref m);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }

            GL.DisableVertexAttribArray(0);
        }
    }
}
