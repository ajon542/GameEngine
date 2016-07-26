using GameEngine.Core.Graphics;
using OpenTK.Graphics.OpenGL;

using NLog;

using GameEngine.Core.Debugging;

namespace GameEngine.Core
{
    public class Renderer : Component
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public Material material;
        public Mesh mesh;

        public Renderer()
        {
            logger.Log(LogLevel.Info, "");
        }

        public void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            if (material == null)
            {
                throw new GameEngineException("material cannot be null");
            }

            if (mesh == null)
            {
                throw new GameEngineException("mesh cannot be null");
            }

            material.Initialize();
            material.SetPositionBuffer(mesh.Vertices.ToArray());
            material.SetNormalBuffer(mesh.Normals.ToArray());
            material.SetTexCoordBuffer(mesh.UV.ToArray());
            material.SetElementBuffer(mesh.Indices.ToArray());

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public void Render(DefaultShaderInput shaderInput)
        {
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

            material.BindVertexArray();

            material.EnableVertexAttribArrays();

            GL.DrawElements(mesh.RenderType, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);

            material.DisableVertexAttribArrays();
        }

        /// <summary>
        /// Cleanup any renderer resources.
        /// </summary>
        public void Destroy()
        {
            logger.Log(LogLevel.Info, "");

            material.Destroy();
        }
    }
}
