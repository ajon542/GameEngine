using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// An example to investigate the performance enhancements acheived from instancing.
    /// </summary>
    public class InstancingExample : Scene
    {
        private uint vertexArrayId;
        private uint vertexbuffer;

        private List<Vector4> vertices = new List<Vector4>
        {
            new Vector4(-0.1f, -0.1f, 0.0f, 1.0f),
            new Vector4( 0.1f, -0.1f, 0.0f, 1.0f),
            new Vector4( 0.1f,  0.1f, 0.0f, 1.0f),
            new Vector4(-0.1f,  0.1f, 0.0f, 1.0f)
        };
        private List<Vector4> colors;
        private List<Vector4> positions;

        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        private int instanceCount = 100;

        public override void Initialize()
        {
            colors = new List<Vector4>();
            positions = new List<Vector4>();

            for (int dep = 0; dep < instanceCount; ++dep)
            {
                for (int col = 0; col < instanceCount; ++col)
                {
                    for (int row = 0; row < instanceCount; ++row)
                    {
                        colors.Add(new Vector4(col % 3, row % 3, dep % 3, 1));
                        positions.Add(new Vector4(col, row, -dep, 0));
                    }
                }
            }

            shaders.Add("default", new ShaderProgram("Core/Shaders/instanced-vert.glsl", "Core/Shaders/instanced-frag.glsl", true));

            GL.GenVertexArrays(1, out vertexArrayId);
            GL.GenBuffers(1, out vertexbuffer);
            GL.BindVertexArray(vertexArrayId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);

            int vertexBufferSize = (vertices.Count * Vector4.SizeInBytes);
            int colorBufferSize = (colors.Count * Vector4.SizeInBytes);
            int instanceBufferSize = (positions.Count * Vector4.SizeInBytes);

            int offset = 0;
            IntPtr totalLength = (IntPtr)(vertexBufferSize + colorBufferSize + instanceBufferSize);
            GL.BufferData(BufferTarget.ArrayBuffer, totalLength, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)vertexBufferSize, vertices.ToArray());
            offset += vertexBufferSize;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)colorBufferSize, colors.ToArray());
            offset += colorBufferSize;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)instanceBufferSize, positions.ToArray());
            offset += instanceBufferSize;

            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, vertexBufferSize);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, vertexBufferSize + colorBufferSize);

            GL.VertexAttribDivisor(1, 1); // index of instance color
            GL.VertexAttribDivisor(2, 1); // index of instance position

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public override void Update()
        {
            MainCamera.Update();

            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            Matrix4 modelViewProjectionMatrix = gameObject.ModelMatrix * (MainCamera.ViewMatrix * MainCamera.ProjectionMatrix);

            GL.UseProgram(shaders["default"].ProgramId);
            GL.BindVertexArray(vertexArrayId);

            shaders["default"].EnableVertexAttribArrays();

            int mvpMatrixId;
            shaders["default"].GetUniform("MVPMatrix", out mvpMatrixId);
            GL.UniformMatrix4(mvpMatrixId, false, ref modelViewProjectionMatrix);

            int numberOfIndices = 4;
            int numberOfInstances = instanceCount * instanceCount * instanceCount;
            GL.DrawArraysInstanced(PrimitiveType.TriangleFan, 0, numberOfIndices, numberOfInstances);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
