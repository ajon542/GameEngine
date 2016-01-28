﻿using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.Graphics
{
    public static class Vao
    {
        // TODO: Fix these, they don't belong in the batch class.
        public const int Cube = 0;
        public const int Cone = 1;
        public const int Count = 2;
    }

    public static class Vbo
    {
        public const int Vertices = 0;
        public const int Colours = 1;
        public const int Elements = 2;
        public const int Count = 3;
    }

    // TODO: Improve the batch class, it isn't actualyl batching anything :)
    // In order to improve this class we are going to need to remove the shader specific
    // stuff from the class.
    // A shader is most often going to have vertices, colours, normals and texture coords.
    // The vertices need to be passed to the shader so we should extend the Vbo enum to
    // account for all the types.
    // Currently, the ShaderProgram class handles the GenBuffer, Attribute and Uniform
    // locations. This may present a problem when trying to separate the shader and batch.
    // The usage we are looking for is:
    // - Specify a shader
    // - Render batch
    // We currently have:
    // - Render batch with hardcoded shader

    class Batch
    {
        private PrimitiveType renderType;

        private uint[] vertexArrObjects;
        private uint[] vertexBufObjects;

        private int indicesCount;

        public Batch(Mesh mesh)
        {
            renderType = mesh.RenderType;

            vertexArrObjects = new uint[Vao.Count];
            vertexBufObjects = new uint[Vbo.Count];

            GL.GenVertexArrays(Vao.Count, vertexArrObjects);
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.GenBuffers(Vbo.Count, vertexBufObjects);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufObjects[Vbo.Vertices]);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufObjects[Vbo.Colours]);
            GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            GL.ColorPointer(3, ColorPointerType.Float, 0, 0);
            GL.EnableClientState(ArrayCap.ColorArray);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vertexBufObjects[Vbo.Elements]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
    }

    class ShaderBatch
    {
        private PrimitiveType renderType;

        private uint[] vertexArrObjects;
        private int indexBufObject;
        private int indicesCount;

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;

        public ShaderBatch(Mesh mesh)
        {
            renderType = mesh.RenderType;

            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));

            vertexArrObjects = new uint[Vao.Count];

            GL.GenVertexArrays(Vao.Count, vertexArrObjects);
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.GenBuffers(1, out indexBufObject);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");
            colourBuffer = shaders["default"].GetBuffer("VertexColor");
            colourAttr = shaders["default"].GetAttribute("VertexColor");

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);
            //GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colourBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(colourAttr, 3, VertexAttribPointerType.Float, true, 0, 0);
            // TODO: This EnableClientState appears to make the GameScene example crash.
            //GL.EnableClientState(ArrayCap.ColorArray);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);
        }

        public void Render(Matrix4 mat)
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);

            shaders["default"].EnableVertexAttribArrays();

            // TODO: Matrix is kinda weird in here...
            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref mat);
            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
