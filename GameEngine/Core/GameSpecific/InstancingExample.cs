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
            new Vector4(-1.0f, -1.0f, 0.0f, 1.0f),
            new Vector4( 1.0f, -1.0f, 0.0f, 1.0f),
            new Vector4( 1.0f,  1.0f, 0.0f, 1.0f),
            new Vector4(-1.0f,  1.0f, 0.0f, 1.0f)
        };

        private List<Vector4> colors = new List<Vector4>
        {
            new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
            new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
            new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
            new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
            new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
            new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
        };

        private List<Vector4> positions = new List<Vector4>
        {
            new Vector4(-2.0f, -2.0f,  -5.0f, 0.0f),
            new Vector4( 2.0f, -2.0f,  -5.0f, 0.0f),
            new Vector4( 2.0f,  2.0f,  -5.0f, 0.0f),
            new Vector4(-2.0f,  2.0f,  -5.0f, 0.0f),
            new Vector4(-1.5f, -2.5f, -10.0f, 0.0f),
            new Vector4( 1.5f, -2.5f, -10.0f, 0.0f),
            new Vector4( 1.5f,  2.5f, -10.0f, 0.0f),
            new Vector4(-1.5f,  2.5f, -10.0f, 0.0f)
        };

        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
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
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);
            GL.BindVertexArray(vertexArrayId);

            shaders["default"].EnableVertexAttribArrays();

            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref gameObject.ModelViewProjectionMatrix);

            int numberOfIndices = 4;
            int numberOfInstances = 8;
            GL.DrawArraysInstanced(PrimitiveType.TriangleFan, 0, numberOfIndices, numberOfInstances);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
