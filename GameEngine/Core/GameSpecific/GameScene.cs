using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.GameSpecific
{
    class GameScene : Scene
    {
        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        private string activeShader = "default";
        private int ibo_elements;

        private Camera cam = new Camera();

        private Mesh mesh;
        private Vector3[] colorData;

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

            // Add default shaders.
            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));

            // Add our cube behaviour.
            BehaviourComponent cubeBehaviour = new CubeBehaviour();
            gameObject.AddComponent<BehaviourComponent>(cubeBehaviour);
            cubeBehaviour.Initialize();

            // TODO: This still doesn't belong here...
            colorData = new[]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f)
            };

            mesh = gameObject.GetComponent<Mesh>() as Mesh;
        }

        public override void Update()
        {
            // TODO: A little inefficient
            BehaviourComponent component = gameObject.GetComponent<BehaviourComponent>() as BehaviourComponent;

            if (component != null)
            {
                component.Update();
            }

            // Update...
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() *
                Matrix4.CreatePerspectiveFieldOfView(1, 1200 / (float)800, 1.0f, 1000.0f);
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

            Vector3[] vertices = mesh.Vertices.ToArray();
            int[] triangles = mesh.Triangles.ToArray();

            // Bind the buffers.
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mesh.Vertices.Count * Vector3.SizeInBytes), vertices,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData,
                    BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                    true, 0, 0);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mesh.Triangles.Count * sizeof(int)), triangles,
                BufferUsageHint.StaticDraw);

            GL.UseProgram(shaders[activeShader].ProgramId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void KeyDown(KeyEventArgs key)
        {
            BehaviourComponent component = gameObject.GetComponent<BehaviourComponent>() as BehaviourComponent;

            if (component != null)
            {
                component.KeyDown(key);
            }
        }

        public override void Render()
        {
            // Render...
            shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;

            GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, mesh.Triangles.Count, DrawElementsType.UnsignedInt,
                            indiceat * sizeof(uint));

            shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
        }
    }

    // TODO: These will be created by the user of the game engine.
    public class SceneA : Scene
    {
        private int vbo;

        public override void Initialize()
        {
            CreateVertexBuffer();
        }

        public override void Render()
        {
            GL.PushMatrix();

            GL.Color3(Color.Yellow);
            GL.PointSize(5);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Points, 0, 3);
            GL.DisableVertexAttribArray(0);

            GL.PopMatrix();
        }

        private void CreateVertexBuffer()
        {
            Vector3[] vertices = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                float xpos = i / 5f;
                vertices[i] = new Vector3(xpos, 0, -1);
            }

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                                   new IntPtr(vertices.Length * Vector3.SizeInBytes),
                                   vertices, BufferUsageHint.StaticDraw);
        }
    }

    public class SceneB : Scene
    {
        public override void Render()
        {
            GL.PushMatrix();

            GL.Color3(Color.Blue);

            GL.Translate(0, 0, -5);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 1, 1);
            GL.End();

            GL.PopMatrix();
        }
    }
}
