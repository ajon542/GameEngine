﻿using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.Graphics
{
    public static class Vao
    {
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

    class Batch
    {
        private uint[] vertexArrObjects;
        private uint[] vertexBufObjects;

        private int indicesCount;

        public Batch(Mesh mesh)
        {
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
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
