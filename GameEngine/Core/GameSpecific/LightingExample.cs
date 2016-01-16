﻿using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    // Ambient lighting is completed.
    // In order to complete the directional lighting, I need to learn about
    // the normal matrix and how to calculate it, then pass it to the vertex shader.
    public class LightingExample : Scene
    {
        private uint vertexArrObject;
        private int indicesCount;
        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;
        private uint normalBuffer;
        private int normalAttr;
        private uint elementBuffer;

        private PrimitiveType renderType = PrimitiveType.Triangles;

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        private GameObject gameObject = new GameObject();
        private Camera cam = new Camera();
        private Mesh mesh = new Cube();

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/directional-vert.glsl", "Core/Shaders/directional-frag.glsl", true));
            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");
            colourBuffer = shaders["default"].GetBuffer("VertexColor");
            colourAttr = shaders["default"].GetAttribute("VertexColor");
            normalBuffer = shaders["default"].GetBuffer("VertexNormal");
            normalAttr = shaders["default"].GetAttribute("VertexNormal");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            Vector3[] normals = mesh.Normals.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr normalsLength = (IntPtr)(normals.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colourBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(colourAttr, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normalsLength, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(normalAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);
        }

        public float xRot = 0;
        public float yRot = 0;
        public float zRot = 0;
        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            if (keyboard[Key.X])
            {
                xRot += 0.1f;
            }
            if (keyboard[Key.Y])
            {
                yRot += 0.1f;
            }
            if (keyboard[Key.Z])
            {
                zRot += 0.1f;
            }

            gameObject.Transform.Position = new Vector3(0, 0, -5);
            gameObject.Transform.Rotation = new Quaternion(xRot, yRot, zRot, 1);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.ViewMatrix * ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            // TODO: Determine good values for the directional lighting.
            GL.Uniform3(shaders["default"].GetUniform("Ambient"), new Vector3(0.5f, 0.5f, 0.5f));
            GL.Uniform3(shaders["default"].GetUniform("LightColor"), new Vector3(1, 1, 1));
            GL.Uniform3(shaders["default"].GetUniform("LightDirection"), new Vector3(1, 1, 1));
            GL.Uniform3(shaders["default"].GetUniform("HalfVector"), new Vector3(0, 0, 0));
            GL.Uniform1(shaders["default"].GetUniform("Shininess"), 1.0f);
            GL.Uniform1(shaders["default"].GetUniform("Strength"), 1.0f);

            // TODO: Calculate the normal matrix correctly.
            // See: http://www.lighthouse3d.com/tutorials/glsl-12-tutorial/the-normal-matrix/
            Matrix4 modelViewMatrix = gameObject.ModelMatrix * cam.ViewMatrix;
            GL.UniformMatrix4(shaders["default"].GetUniform("NormalMatrix"), false, ref modelViewMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}