using System;
using System.Collections.Generic;
using GameEngine.Core.Utilities.ObjParser;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameEngine.Core
{
    public class Renderer : Component
    {
        private uint vertexArrObject;
        private int indicesCount;
        private uint positionBuffer;
        private int positionAttr;
        private uint normalBuffer;
        private int normalAttr;
        private uint elementBuffer;

        public Material material;
        public Mesh mesh;

        public Renderer()
        {
        }

        public void Initialize()
        {
            positionBuffer = material.GetBuffer("position");
            positionAttr = material.GetAttribute("position");
            normalBuffer = material.GetBuffer("normal");
            normalAttr = material.GetAttribute("normal");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] normals = mesh.Normals.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr normalsLength = (IntPtr)(normals.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normalsLength, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(normalAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public void Render(Matrix4 modelViewMatrix, Matrix4 projectionMatrix)
        {
            GL.UseProgram(material.shaderProgram.ProgramId);

            GL.BindVertexArray(vertexArrObject);

            material.shaderProgram.EnableVertexAttribArrays();

            GL.UniformMatrix4(material.GetUniform("mv_matrix"), false, ref modelViewMatrix);
            GL.UniformMatrix4(material.GetUniform("proj_matrix"), false, ref projectionMatrix);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            material.shaderProgram.DisableVertexAttribArrays();
        }
    }
}
