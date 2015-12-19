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
}
