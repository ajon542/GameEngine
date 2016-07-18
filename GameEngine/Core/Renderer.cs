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
        public Material material;
        public Mesh mesh;

        public Renderer()
        {
        }

        public void Initialize()
        {
            material.Initialize();
            material.SetPositionBuffer(mesh.Vertices.ToArray());
            material.SetNormalBuffer(mesh.Normals.ToArray());
            material.SetElementBuffer(mesh.Indices.ToArray());

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public void Render(Matrix4 modelViewMatrix, Matrix4 projectionMatrix)
        {
            material.UseProgram();

            material.BindVertexArray();

            material.EnableVertexAttribArrays();

            material.SetMatrix4("mv_matrix", modelViewMatrix);
            material.SetMatrix4("proj_matrix", projectionMatrix);

            GL.DrawElements(mesh.RenderType, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);

            material.DisableVertexAttribArrays();
        }

        /// <summary>
        /// Cleanup any renderer resources.
        /// </summary>
        public void Destroy()
        {
            material.Destroy();
        }
    }
}
