using System;
using System.Collections.Generic;
using GameEngine.Core.Graphics;
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

        public void Render(DefaultShaderInput shaderInput)
        {
            material.UseProgram();

            material.BindVertexArray();

            material.EnableVertexAttribArrays();

            // Set default matrices.
            material.SetMatrix4("MATRIX_MVP", shaderInput.MatrixMVP);
            material.SetMatrix4("MATRIX_MV", shaderInput.MatrixMV);
            material.SetMatrix4("MATRIX_V", shaderInput.MatrixV);
            material.SetMatrix4("MATRIX_P", shaderInput.MatrixP);
            material.SetMatrix4("MATRIX_VP", shaderInput.MatrixVP);
            material.SetMatrix4("Object2World", shaderInput.Object2World);
            material.SetMatrix4("World2Object", shaderInput.World2Object);
            material.SetVector3("WorldCameraPos", shaderInput.WorldCameraPos);

            // Set default lights.
            material.SetVector3("LightPosition", shaderInput.LightPosition);
            material.SetVector4("LightColor", shaderInput.LightColor);

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
