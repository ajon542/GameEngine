using System;
using System.Collections.Generic;
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
            GL.Color3(0, 0, 0);
            GL.PointSize(5);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Points, 0, 3);
            GL.DisableVertexAttribArray(0);
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

        }
    }
}
