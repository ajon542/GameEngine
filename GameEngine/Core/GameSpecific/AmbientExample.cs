using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    class AmbientExample : Scene
    {
        private uint vertexArrObject;
        private int indicesCount;
        private uint elementBuffer;
        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;

        private GameObject gameObject = new GameObject();
        private Mesh mesh = new Mesh();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/ambient-vert.glsl", "Core/Shaders/ambient-frag.glsl", true));

            ObjFile file = new ObjFile();
            file.Read("Core/GameSpecific/Assets/Mesh/Cube.obj");
            mesh = file.Mesh;

            colourBuffer = shaders["default"].GetBuffer("VertexColor");
            colourAttr = shaders["default"].GetAttribute("VertexColor");
            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");

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

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public override void Update()
        {
            MainCamera.Update();

            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            float lightIntensity = 1.0f;
            Vector3 lightColor = new Vector3(0.5f, 0.5f, 0.5f);
            GL.Uniform1(shaders["default"].GetUniform("LightIntensity"), 1, ref lightIntensity);
            GL.Uniform3(shaders["default"].GetUniform("LightColor"), ref lightColor);
            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref gameObject.ModelViewProjectionMatrix);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
