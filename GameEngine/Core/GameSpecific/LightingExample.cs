using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    // Ambient lighting is completed.
    // In order to complete the directional lighting, I need to learn about
    // the normal matrix and how to calculate it, then pass it to the vertex shader.
    public class LightingExample : Scene
    {
        private static class Vbo
        {
            public const int Vertices = 0;
            public const int Colours = 1;
            public const int Elements = 2;
            public const int Count = 3;
        }

        private uint vertexArrObject;
        private int indicesCount;
        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;
        private uint elementBuffer;

        private PrimitiveType renderType = PrimitiveType.Triangles;

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        private GameObject gameObject = new GameObject();
        private Camera cam = new Camera();
        private Mesh mesh = new Cube();

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/ambient-frag.glsl", true));
            positionBuffer = shaders["default"].GetBuffer("vPosition");
            positionAttr = shaders["default"].GetAttribute("vPosition");
            colourBuffer = shaders["default"].GetBuffer("vColor");
            colourAttr = shaders["default"].GetAttribute("vColor");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, colourBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(colourAttr, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);

            gameObject.Transform.Position = new Vector3(0, 0, -5);
            gameObject.Transform.Rotation = new Quaternion(1, 1, 0, 1);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() * ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            GL.Uniform4(shaders["default"].GetUniform("ambient"), new Vector4(0.5f, 0.5f, 0.5f, 1));
            GL.UniformMatrix4(shaders["default"].GetUniform("mvp"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
