using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    public class Scene
    {
        public virtual void Initialize()
        {

        }

        public virtual void Render()
        {

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

    public class SceneC : Scene
    {
        private GameObject gameObject = new GameObject();
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        private string activeShader = "default";
        private int ibo_elements;
        private Mesh mesh;

        private Camera cam = new Camera();

        private Vector3[] colorData;

        public override void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new ShaderProgram("Core/Shaders/vert.glsl", "Core/Shaders/frag.glsl", true));

            mesh = new Mesh();
            mesh.Vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
            mesh.Vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            int[] indices =
            {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            colorData = new []
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

            mesh.Triangles = new List<int>(indices);

            gameObject.AddComponent<Mesh>(mesh);

            gameObject.Transform.Position = new Vector3(0, 0, -3.0f);
            gameObject.Transform.Rotation = new Quaternion(1, 1, 1, 0.5f);
        }

        public override void Render()
        {
            // TODO: Decouple the update and render.
            // Update...
            // TODO: This matrix stuff must come out of here...
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
}
