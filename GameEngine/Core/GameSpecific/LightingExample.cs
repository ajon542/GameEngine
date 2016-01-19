using System;
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
        private Mesh mesh = new Sphere(1, 1, 25, 25);
        Light activeLight = new Light(new Vector3(0, 3, -11), new Vector3(1.0f, 0.0f, 0.0f));

        public override void Initialize()
        {
            shaders.Add("default", new ShaderProgram("Core/Shaders/blinnphong-vert.glsl", "Core/Shaders/blinnphong-frag.glsl", true));
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

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        private int mouseWheelIndex;
        private int prevX;
        private int prevY;
        private float mouseSensitivity = 0.01f;

        private bool mouseLeftDown;
        private void CameraUpdate()
        {
            var mouse = Mouse.GetState();
            if (mouse[MouseButton.Left])
            {
                if (mouseLeftDown == false)
                {
                    prevX = mouse.X;
                    prevY = mouse.Y;
                }
                mouseLeftDown = true;

                if (prevY > mouse.Y)
                {
                    MainCamera.Move(0, (prevY - mouse.Y) * -mouseSensitivity, 0);
                }
                if (prevY < mouse.Y)
                {
                    MainCamera.Move(0, (mouse.Y - prevY) * mouseSensitivity, 0);
                }
                if (prevX > mouse.X)
                {
                    MainCamera.Move((prevX - mouse.X) * mouseSensitivity, 0, 0);
                }
                if (prevX < mouse.X)
                {
                    MainCamera.Move((mouse.X - prevX) * -mouseSensitivity, 0, 0);
                }
                prevX = mouse.X;
                prevY = mouse.Y;
            }
            else
            {
                mouseLeftDown = false;
            }

            // Handle zoom.
            if (mouseWheelIndex != mouse.Wheel)
            {
                Vector3 vec = MainCamera.LookAt - MainCamera.Position;

                if (mouseWheelIndex > mouse.Wheel)
                {
                    vec *= -0.5f;
                }
                else
                {
                    vec *= 0.5f;
                }

                MainCamera.LookAt = new Vector3(
                    MainCamera.LookAt.X + vec.X,
                    MainCamera.LookAt.Y + vec.Y,
                    MainCamera.LookAt.Z + vec.Z
                    );
                MainCamera.Position = new Vector3(
                    MainCamera.Position.X + vec.X,
                    MainCamera.Position.Y + vec.Y,
                    MainCamera.Position.Z + vec.Z
                    );
                mouseWheelIndex = mouse.Wheel;
                Console.WriteLine("LookAt {0}, Position {1}, Vec {2}", MainCamera.LookAt, MainCamera.Position, vec);
            }
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

            CameraUpdate();

            gameObject.Transform.Position = new Vector3(0, 0, -10);
            gameObject.Transform.Rotation = new Quaternion(xRot, yRot, zRot, 1);
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["default"].ProgramId);

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            Matrix4 viewMatrix = MainCamera.ViewMatrix;

            Vector3 ambientColor = new Vector3(0.1880f, 0.1880f, 0.1880f);
            Vector3 diffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 specularColor = new Vector3(0.1000f, 0.1000f, 0.1000f);
            float specularExponent = 1.0f;

            GL.UniformMatrix4(shaders["default"].GetUniform("ModelViewMatrix"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ViewMatrix"), false, ref viewMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ModelMatrix"), false, ref gameObject.ModelMatrix);
            GL.Uniform3(shaders["default"].GetUniform("material_ambient"), ref ambientColor);
            GL.Uniform3(shaders["default"].GetUniform("material_diffuse"), ref diffuseColor);
            GL.Uniform3(shaders["default"].GetUniform("material_specular"), ref specularColor);
            GL.Uniform1(shaders["default"].GetUniform("material_specExponent"), specularExponent);
            GL.Uniform3(shaders["default"].GetUniform("light_position"), ref activeLight.Position);
            GL.Uniform3(shaders["default"].GetUniform("light_color"), ref activeLight.Color);
            GL.Uniform1(shaders["default"].GetUniform("light_diffuseIntensity"), activeLight.DiffuseIntensity);
            GL.Uniform1(shaders["default"].GetUniform("light_ambientIntensity"), activeLight.AmbientIntensity);

            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
