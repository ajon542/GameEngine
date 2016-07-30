using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    class DiffuseExample : Scene
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
        private Mesh mesh = new Torus(1, 0.3f, 20, 20);
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/ambient-diffuse-vert.glsl", "Core/Shaders/ambient-diffuse-frag.glsl", true));

            shaders["default"].GetBuffer("VertexColor", out colourBuffer);
            shaders["default"].GetAttribute("VertexColor", out colourAttr);
            shaders["default"].GetBuffer("VertexPosition", out positionBuffer);
            shaders["default"].GetAttribute("VertexPosition", out positionAttr);
            shaders["default"].GetBuffer("VertexNormal", out normalBuffer);
            shaders["default"].GetAttribute("VertexNormal", out normalAttr);

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

        public float yRot;
        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            if (keyboard[Key.Y])
            {
                yRot += 0.1f;
            }

            MainCamera.Update();

            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, yRot);
            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            Matrix4 modelViewProjectionMatrix = gameObject.ModelMatrix * (MainCamera.ViewMatrix * MainCamera.ProjectionMatrix);

            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            float lightAmbientIntensity = 0.01f;
            float lightDiffuseIntensity = 0.75f;
            Vector3 lightColor = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 lightDirection = new Vector3(1.0f, 0.0f, 0.0f);

            lightDirection.Normalize();

            int lightAmbientIntensityId;
            int lightDiffuseIntensityId;
            int lightColorId;
            int lightDirectionId;
            int modelMatrixId;
            int mVPMatrixId;

            shaders["default"].GetUniform("LightAmbientIntensity", out lightAmbientIntensityId);
            shaders["default"].GetUniform("LightDiffuseIntensity", out lightDiffuseIntensityId);
            shaders["default"].GetUniform("LightColor", out lightColorId);
            shaders["default"].GetUniform("LightDirection", out lightDirectionId);

            shaders["default"].GetUniform("ModelMatrix", out modelMatrixId);
            shaders["default"].GetUniform("MVPMatrix", out mVPMatrixId);

            GL.Uniform1(lightAmbientIntensityId, 1, ref lightAmbientIntensity);
            GL.Uniform1(lightDiffuseIntensityId, 1, ref lightDiffuseIntensity);
            GL.Uniform3(lightColorId, ref lightColor);
            GL.Uniform3(lightDirectionId, ref lightDirection);

            GL.UniformMatrix4(modelMatrixId, false, ref gameObject.ModelMatrix);
            GL.UniformMatrix4(mVPMatrixId, false, ref modelViewProjectionMatrix);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
