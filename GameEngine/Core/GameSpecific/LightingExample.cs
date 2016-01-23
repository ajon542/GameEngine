using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
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
        private int uvAttr;
        private uint uvBuffer;

        private int textureId;

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        private GameObject gameObject = new GameObject();
        //private Mesh mesh = new Sphere(1, 1, 25, 25);
        //private Mesh mesh = new Cube();
        //private Mesh mesh = new Torus(2.0f, 0.5f, 50, 50);
        private Mesh mesh = new OctahedronSphere(6, 2);
        Light activeLight = new Light(new Vector3(5, 5, -5), new Vector3(1.0f, 1.0f, 1.0f));

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/blinnphong-vert.glsl", "Core/Shaders/blinnphong-frag.glsl", true));
            textureId = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/Planet.png");

            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");
            //colourBuffer = shaders["default"].GetBuffer("VertexColor");
            //colourAttr = shaders["default"].GetAttribute("VertexColor");
            normalBuffer = shaders["default"].GetBuffer("VertexNormal");
            normalAttr = shaders["default"].GetAttribute("VertexNormal");
            uvBuffer = shaders["default"].GetBuffer("VertexUV");
            uvAttr = shaders["default"].GetAttribute("VertexUV");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            Vector3[] normals = mesh.Normals.ToArray();
            Vector2[] uv = mesh.UV.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr normalsLength = (IntPtr)(normals.Length * Vector3.SizeInBytes);
            IntPtr uvsLength = (IntPtr)(uv.Length * Vector2.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, colourBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(colourAttr, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normalsLength, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(normalAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, uvsLength, uv, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(uvAttr, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
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

            MainCamera.Update();

            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.Transform.Rotation = new Quaternion(xRot, yRot, zRot, 1);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Uniform1(shaders["default"].GetUniform("mainTexture"), TextureUnit.Texture0 - TextureUnit.Texture0);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            Matrix4 viewMatrix = MainCamera.ViewMatrix;

            Vector3 ambientColor = new Vector3(0.1880f, 0.1880f, 0.1880f);
            Vector3 diffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 specularColor = new Vector3(1.0f, 0.0f, 00f);
            float specularExponent = 10.0f;

            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ViewMatrix"), false, ref viewMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ModelMatrix"), false, ref gameObject.ModelMatrix);
            GL.Uniform3(shaders["default"].GetUniform("MaterialAmbient"), ref ambientColor);
            GL.Uniform3(shaders["default"].GetUniform("MaterialDiffuse"), ref diffuseColor);
            GL.Uniform3(shaders["default"].GetUniform("MaterialSpecular"), ref specularColor);
            GL.Uniform1(shaders["default"].GetUniform("MaterialSpecExponent"), specularExponent);
            GL.Uniform3(shaders["default"].GetUniform("LightPosition"), ref activeLight.Position);
            GL.Uniform3(shaders["default"].GetUniform("LightColor"), ref activeLight.Color);
            GL.Uniform1(shaders["default"].GetUniform("LightDiffuseIntensity"), activeLight.DiffuseIntensity);
            GL.Uniform1(shaders["default"].GetUniform("LightAmbientIntensity"), activeLight.AmbientIntensity);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
