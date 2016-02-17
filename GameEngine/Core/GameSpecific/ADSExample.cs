using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    class ADSExample : Scene
    {
        private uint vertexArrObject;
        private int indicesCount;
        private uint elementBuffer;
        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;
        private uint normalBuffer;
        private int normalAttr;

        private GameObject gameObject = new GameObject();
        private Mesh mesh = new Quad();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            MainCamera.Position = new Vector3(0, 10, 100);

            shaders.Add("default", new ShaderProgram("Core/Shaders/ads-vert.glsl", "Core/Shaders/ads-frag.glsl", true));

            colourBuffer = shaders["default"].GetBuffer("VertexColor");
            colourAttr = shaders["default"].GetAttribute("VertexColor");
            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");
            normalBuffer = shaders["default"].GetBuffer("VertexNormal");
            normalAttr = shaders["default"].GetAttribute("VertexNormal");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] normals = mesh.Normals.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr normalsLength = (IntPtr)(normals.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normalsLength, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(normalAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

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

            gameObject.Transform.Position = new Vector3(0, -10, 0);
            gameObject.Transform.Scale = new Vector3(40, 40, 1);
            gameObject.Transform.Rotation = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), DegreesToRadians(-90));
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        // TODO: Move into a helper class.
        private float DegreesToRadians(float degrees)
        {
            return (degrees * (float)Math.PI) / 180.0f;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            float lightAmbientIntensity = 0.01f;
            float lightDiffuseIntensity = 0.75f;
            Vector3 lightColor = new Vector3(1.0f, 1.0f, 1.0f);
            // TODO: The light needs a position.
            Vector3 lightDirection = new Vector3(0, -5, -5);
            lightDirection.Normalize();
            float specularIntensity = 0.7f;
            float specularPower = 5;

            Matrix4 eyeWorldPos = MainCamera.ViewMatrix;
            GL.Uniform1(shaders["default"].GetUniform("LightAmbientIntensity"), 1, ref lightAmbientIntensity);
            GL.Uniform1(shaders["default"].GetUniform("LightDiffuseIntensity"), 1, ref lightDiffuseIntensity);
            GL.Uniform3(shaders["default"].GetUniform("LightColor"), ref lightColor);
            GL.Uniform3(shaders["default"].GetUniform("LightDirection"), ref lightDirection);

            GL.Uniform1(shaders["default"].GetUniform("SpecularIntensity"), 1, ref specularIntensity);
            GL.Uniform1(shaders["default"].GetUniform("SpecularPower"), 1, ref specularPower);

            // TODO: EyeWorldPos is not a Matrix4 look into this.
            GL.UniformMatrix4(shaders["default"].GetUniform("EyeWorldPos"), false, ref eyeWorldPos);
            GL.UniformMatrix4(shaders["default"].GetUniform("ModelMatrix"), false, ref gameObject.ModelMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref gameObject.ModelViewProjectionMatrix);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
