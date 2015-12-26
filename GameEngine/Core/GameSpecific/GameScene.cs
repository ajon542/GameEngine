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
        private Vector3[] colorData;
        private Vector3[] colorData2;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        public override void Initialize()
        {
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");
            logger.Info("Sample informational message");
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");

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
            Behaviour cubeBehaviour = new CubeBehaviour();
            gameObject.AddComponent<Behaviour>(cubeBehaviour);
            cubeBehaviour.Initialize();

            Behaviour cubeBehaviour2 = new CubeBehaviour();
            gameObject2.AddComponent<Behaviour>(cubeBehaviour2);
            cubeBehaviour2.Initialize();

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

            colorData2 = new[]
            {
                new Vector3(1f, 0f, 0f),
            };

            mesh = gameObject.GetComponent<Mesh>() as Mesh;
            mesh2 = gameObject2.GetComponent<Mesh>() as Mesh;

            Vector3[] vertices = mesh.Vertices.ToArray();
            int[] triangles = mesh.Triangles.ToArray();

            Vector3[] vertices2 = mesh2.Vertices.ToArray();
            int[] triangles2 = mesh2.Triangles.ToArray();

            // Bind vertices.
            IntPtr vertexBufferSize = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            // Bind the color.
            IntPtr colorBufferSize = (IntPtr)(colorData.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
            GL.BufferData(BufferTarget.ArrayBuffer, colorBufferSize, colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            // Bind the indices.
            IntPtr trianglesBufferSize = (IntPtr)(triangles.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, trianglesBufferSize, triangles, BufferUsageHint.StaticDraw);

            // gameobject 2

            // Bind vertices.
            IntPtr vertexBufferSize2 = (IntPtr)(vertices2.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize2, vertices2, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float,
                false, 0, 0);

            // Bind the color.
            IntPtr colorBufferSize2 = (IntPtr)(colorData2.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders["gameobject2"].GetBuffer("vColor"));
            GL.BufferData(BufferTarget.ArrayBuffer, colorBufferSize2, colorData2, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders["gameobject2"].GetAttribute("vColor"), 3, VertexAttribPointerType.Float,
                true, 0, 0);

            // Bind the indices.
            IntPtr trianglesBufferSize2 = (IntPtr)(triangles2.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements2);
            GL.BufferData(BufferTarget.ElementArrayBuffer, trianglesBufferSize2, triangles2, BufferUsageHint.StaticDraw);

            GL.UseProgram(shaders[activeShader].ProgramId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Update()
        {
            // TODO: A little inefficient
            Behaviour component = gameObject.GetComponent<Behaviour>() as Behaviour;

            if (component != null)
            {
                component.Update();
            }

            component = gameObject2.GetComponent<Behaviour>() as Behaviour;

            if (component != null)
            {
                component.Update();
            }

            // Update...
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = cam.GetViewMatrix() *
                Matrix4.CreatePerspectiveFieldOfView(1, 1200 / (float)800, 1.0f, 1000.0f);
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;

            gameObject2.Transform.Position = new Vector3(2, 0, -3.0f);
            gameObject2.CalculateModelMatrix();
            gameObject2.ViewProjectionMatrix = cam.GetViewMatrix() *
                Matrix4.CreatePerspectiveFieldOfView(1, 1200 / (float)800, 1.0f, 1000.0f);
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

            GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref gameObject.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, mesh.Triangles.Count, DrawElementsType.UnsignedInt,
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

            GL.UniformMatrix4(shaders["gameobject2"].GetUniform("modelview"), false, ref gameObject2.ModelViewProjectionMatrix);
            GL.DrawElements(BeginMode.Triangles, mesh2.Triangles.Count, DrawElementsType.UnsignedInt,
                            indiceat * sizeof(uint));

            shaders["gameobject2"].DisableVertexAttribArrays();

            GL.Flush();
        }

        public override void KeyDown(KeyEventArgs key)
        {
            Behaviour component = gameObject.GetComponent<Behaviour>() as Behaviour;

            if (component != null)
            {
                component.KeyDown(key);
            }
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
