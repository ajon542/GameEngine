using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GameEngine.Core.Debugging;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using NLog;
using NLog.Targets;

namespace GameEngine.Core.GameSpecific
{
    /// <summary>
    /// Example of creating a game specific scene.
    /// </summary>
    class GameScene : Scene
    {
        private GameObject gameObject = new GameObject();
        private GameObject gameObject2 = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        private string activeShader = "default";
        private int ibo_elements;
        private int ibo_elements2;

        private Camera cam = new Camera();

        private Mesh mesh;
        private Mesh mesh2;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        public override void Initialize()
        {
            // Set up depth test and face culling.
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // Generate buffer objects.
            GL.GenBuffers(1, out ibo_elements);
            GL.GenBuffers(1, out ibo_elements2);

            // Add default shaders.
            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));
            shaders.Add("gameobject2", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));

            // Add our cube behaviour.
            CubeBehaviour cubeBehaviour = new CubeBehaviour();
            cubeBehaviour.Colour = new Vector3(1, 0, 0);
            gameObject.AddComponent<Behaviour>(cubeBehaviour);
            cubeBehaviour.Initialize();

            CubeBehaviour cubeBehaviour2 = new CubeBehaviour();
            cubeBehaviour2.Colour = new Vector3(0, 0, 1);
            gameObject2.AddComponent<Behaviour>(cubeBehaviour2);
            cubeBehaviour2.Initialize();

            mesh = gameObject.GetComponent<Mesh>();
            mesh2 = gameObject2.GetComponent<Mesh>();

            Vector3[] vertices = mesh.Vertices.ToArray();
            int[] indices = mesh.Indices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();

            Vector3[] vertices2 = mesh2.Vertices.ToArray();
            int[] indices2 = mesh2.Indices.ToArray();
            Vector3[] colours2 = mesh2.Colours.ToArray();

            // Bind vertices.
            IntPtr vertexBufferSize = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            // Bind the color.
            IntPtr colorBufferSize = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
            GL.BufferData(BufferTarget.ArrayBuffer, colorBufferSize, colours, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            // Bind the indices.
            IntPtr indicesBufferSize = (IntPtr)(indices.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesBufferSize, indices, BufferUsageHint.StaticDraw);

            // gameobject 2

            // Bind vertices.
            IntPtr vertexBufferSize2 = (IntPtr)(vertices2.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize2, vertices2, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            // Bind the color.
            IntPtr colorBufferSize2 = (IntPtr)(colours2.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vColor"));
            GL.BufferData(BufferTarget.ArrayBuffer, colorBufferSize2, colours2, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            // Bind the indices.
            IntPtr indicesBufferSize2 = (IntPtr)(indices2.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements2);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesBufferSize2, indices2, BufferUsageHint.StaticDraw);

            GL.UseProgram(shaders[activeShader].ProgramId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Update()
        {
            // TODO: A little inefficient
            Behaviour component = gameObject.GetComponent<Behaviour>();

            if (component != null)
            {
                component.Update();
            }

            component = gameObject2.GetComponent<Behaviour>();

            if (component != null)
            {
                component.Update();
            }

            // Update...
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() * ViewProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

            gameObject2.Transform.Position = new Vector3(2, 0, -5.0f);
            gameObject2.CalculateModelMatrix();
            gameObject2.ViewProjectionMatrix = cam.GetViewMatrix() * ViewProjectionMatrix;
            gameObject2.ModelViewProjectionMatrix = gameObject2.ModelMatrix * gameObject2.ViewProjectionMatrix;
        }

        public override void Render()
        {
            // Render...
            GL.UseProgram(shaders[activeShader].ProgramId);
            int indiceat = 0;

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            shaders[activeShader].EnableVertexAttribArrays();

            GL.UniformMatrix4(shaders[activeShader].GetUniform("mvp"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt,
                            indiceat * sizeof(uint));

            shaders[activeShader].DisableVertexAttribArrays();

            // ---------------------------------

            GL.UseProgram(shaders["gameobject2"].ProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vPosition"));
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vColor"));
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            shaders["gameobject2"].EnableVertexAttribArrays();

            GL.UniformMatrix4(shaders["gameobject2"].GetUniform("mvp"), false, ref gameObject2.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, mesh2.Indices.Count, DrawElementsType.UnsignedInt,
                            indiceat * sizeof(uint));

            shaders["gameobject2"].DisableVertexAttribArrays();
        }
    }
}
