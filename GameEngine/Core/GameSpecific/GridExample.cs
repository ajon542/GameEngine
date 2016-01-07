using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class GridExample : Scene
    {
        private int ibo_elements;
        private Grid grid = new Grid();
        private Camera cam = new Camera();
        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));

            Vector3[] vertices = grid.Vertices.ToArray();
            int[] indices = grid.Indices.ToArray();
            Vector3[] colours = grid.Colours.ToArray();

            // Bind vertices.
            IntPtr vertexBufferSize = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["default"].GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["default"].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            // Bind the color.
            IntPtr colorBufferSize = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["default"].GetBuffer("vColor"));
            GL.BufferData(BufferTarget.ArrayBuffer, colorBufferSize, colours, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["default"].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            // Bind the indices.
            IntPtr indicesBufferSize = (IntPtr)(indices.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesBufferSize, indices, BufferUsageHint.StaticDraw);
        }

        public override void Update()
        {
            var keyboard = Keyboard.GetState();
            if(keyboard[Key.W])
            {
                // TODO: This cam is moving in the z direction when it should be y.
                cam.Move(0f, 0.1f, 0f);
            }
            if (keyboard[Key.S])
            {
                cam.Move(0f, -0.1f, 0f);
            }
            if (keyboard[Key.A])
            {
                cam.Move(-0.1f, 0f, 0f);
            }
            if (keyboard[Key.D])
            {
                cam.Move(0.1f, 0f, 0f);
            }
            if (keyboard[Key.Z])
            {
                cam.Move(0f, 0f, -0.1f);
            }
            if (keyboard[Key.X])
            {
                cam.Move(0f, 0f, 0.1f);
            }

            if (keyboard[Key.R])
            {
                cam.AddRotation(0.1f, 0);
            }
            if (keyboard[Key.T])
            {
                cam.AddRotation(-0.1f, 0f);
            }



            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() * ViewProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            // Render...
            GL.UseProgram(shaders["default"].ProgramId);
            int indiceat = 0;

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["default"].GetBuffer("vPosition"));
            GL.VertexAttribPointer(shaders["default"].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["default"].GetBuffer("vColor"));
            GL.VertexAttribPointer(shaders["default"].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            shaders["default"].EnableVertexAttribArrays();

            GL.UniformMatrix4(shaders["default"].GetUniform("mvp"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Lines, grid.Indices.Count, DrawElementsType.UnsignedInt,
                            indiceat * sizeof(uint));

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
