﻿using System;
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
        uint vertexArrayId;
        uint vertexbuffer;

        float[] vertices =
        {
            -1.0f, -1.0f, 0.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f,
             1.0f,  1.0f, 0.0f, 1.0f,
            -1.0f,  1.0f, 0.0f, 1.0f
        };

        float[] colors =
        {
            1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f
        };

        float[] positions =
        {
            -2.0f, -2.0f, 0.0f, 0.0f,
             2.0f, -2.0f, 0.0f, 0.0f,
             2.0f,  2.0f, 0.0f, 0.0f,
            -2.0f,  2.0f, 0.0f, 0.0f,
            -1.5f, -2.5f, 0.0f, 0.0f,
             1.5f, -2.5f, 0.0f, 0.0f,
             1.5f,  2.5f, 0.0f, 0.0f,
            -1.5f,  2.5f, 0.0f, 0.0f
        };

        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        int offset;
        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/instanced-vert.glsl", "Core/Shaders/instanced-frag.glsl", true));

            GL.GenVertexArrays(1, out vertexArrayId);
            GL.GenBuffers(1, out vertexbuffer);
            GL.BindVertexArray(vertexArrayId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);

            int vertexBufferSize = (vertices.Length * sizeof(float));
            int colorBufferSize = (colors.Length * sizeof(float));
            int instanceBufferSize = (positions.Length * sizeof(float));

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferSize + colorBufferSize + instanceBufferSize), IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)vertexBufferSize, vertices);
            offset += vertexBufferSize;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)colorBufferSize, colors);
            offset += colorBufferSize;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)instanceBufferSize, positions);
            offset += instanceBufferSize;

            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, vertexBufferSize);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, vertexBufferSize + colorBufferSize);

            GL.VertexAttribDivisor(1, 1); // index of instance color
            GL.VertexAttribDivisor(2, 1); // index of instance position
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);
            GL.BindVertexArray(vertexArrayId);

            shaders["default"].EnableVertexAttribArrays();

            int numberOfIndices = 4;
            int numberOfInstances = 8;
            GL.DrawArraysInstanced(PrimitiveType.TriangleFan, 0, numberOfIndices, numberOfInstances);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
