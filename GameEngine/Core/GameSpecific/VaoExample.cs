﻿using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
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

    public class VaoExample : Scene
    {
        private uint[] vertexArrObjects;
        private uint[] vertexBufObjects;

        private GameObject gameObject = new GameObject();

        private int[] cubeIndices;

        private void InitVbo(Vector3[] vertices, Vector3[] colours, int[] indices)
        {
            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));

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

        public override void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Add our cube behaviour.
            gameObject.AddComponent<Behaviour>(new CubeBehaviour { Colour = new Vector3(1, 0, 0) });
            gameObject.GetComponent<CubeBehaviour>().Initialize();

            // Store the array lengths.
            Vector3[] cubeVerts = gameObject.GetComponent<Mesh>().Vertices.ToArray();
            Vector3[] cubeColours = gameObject.GetComponent<Mesh>().Colours.ToArray();
            cubeIndices = gameObject.GetComponent<Mesh>().Indices.ToArray();

            vertexArrObjects = new uint[Vao.Count];
            vertexBufObjects = new uint[Vbo.Count];

            GL.GenVertexArrays(Vao.Count, vertexArrObjects);

            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.GenBuffers(Vbo.Count, vertexBufObjects);
            InitVbo(cubeVerts, cubeColours, cubeIndices);

            GL.BindVertexArray(vertexArrObjects[Vao.Cone]);
            GL.GenBuffers(Vbo.Count, vertexBufObjects);
            cubeColours[0] = new Vector3(0, 1, 0);
            cubeColours[2] = new Vector3(0, 1, 0);
            cubeColours[4] = new Vector3(0, 1, 0);
            cubeColours[6] = new Vector3(0, 1, 0);
            InitVbo(cubeVerts, cubeColours, cubeIndices);
        }

        public override void Render()
        {
            GL.PushMatrix();
            GL.Translate(0, 0, -3);
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.DrawElements(PrimitiveType.Triangles, cubeIndices.Length, DrawElementsType.UnsignedInt, 0);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(2, 0, -3);
            GL.BindVertexArray(vertexArrObjects[Vao.Cone]);
            GL.DrawElements(PrimitiveType.Triangles, cubeIndices.Length, DrawElementsType.UnsignedInt, 0);
            GL.PopMatrix();
        }
    }
}