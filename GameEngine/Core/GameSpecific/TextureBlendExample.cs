using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    public class TextureBlendExample : Scene
    {
        private uint vertexArrayId;
        private uint vertexBuffer;
        private int vertexAttribute;
        private uint uvBuffer;
        private int uvAttribute;

        /// <summary>
        /// Texture sampler uniform handle in fragment shader.
        /// </summary>
        private int textureSamplerUniform;

        /// <summary>
        /// Texture sampler uniform handle in fragment shader.
        /// </summary>
        private int textureSamplerUniform1;

        /// <summary>
        /// Loaded texture identifier.
        /// </summary>
        private int textureId;

        /// <summary>
        /// Loaded texture identifier.
        /// </summary>
        private int textureId1;

        /// <summary>
        /// Model-view-projection matrix uniform handle.
        /// </summary>
        private int mvpUniform;

        /// <summary>
        /// Reference to the game object mesh component.
        /// </summary>
        private Mesh mesh;

        /// <summary>
        /// The game object containing the mesh.
        /// </summary>
        GameObject gameObject = new GameObject();

        /// <summary>
        /// Storage for the shader programs.
        /// </summary>
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            // TODO: AddComponent must be called before Initialize.. It shouldn't be this fragile.
            gameObject.AddComponent<QuadBehaviour>(new QuadBehaviour());
            gameObject.GetComponent<QuadBehaviour>().Initialize();

            mesh = gameObject.GetComponent<Mesh>();

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector2[] uv = mesh.UV.ToArray();

            // Add default shaders.
            shaders.Add("texture", new ShaderProgram("Core/Shaders/texture-blend-vert.glsl", "Core/Shaders/texture-blend-frag.glsl", true));

            // Load the texture.
            textureId = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/UV-Template.bmp");
            textureId1 = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/Nuclear-Symbol.bmp");

            // Generate a VAO and set it as the current one.
            GL.GenVertexArrays(1, out vertexArrayId);
            GL.BindVertexArray(vertexArrayId);

            // Get the texture sampler uniform location from the fragment shader.
            textureSamplerUniform = shaders["texture"].GetUniform("textureSampler");
            textureSamplerUniform1 = shaders["texture"].GetUniform("textureSampler1");

            // Get the model-view-projection matrix uniform.
            mvpUniform = shaders["texture"].GetUniform("MVPMatrix");

            // Vertices.

            // Generate 1 buffer, put the resulting identifier in vertexbuffer.
            vertexBuffer = shaders["texture"].GetBuffer("VertexPosition");
            vertexAttribute = shaders["texture"].GetAttribute("VertexPosition");

            // The following commands will talk about our 'vertexbuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            // Give our vertices to OpenGL.
            IntPtr vertexBufferSize = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(vertexAttribute, 3, VertexAttribPointerType.Float, false, 0, 0);

            // UVs.

            // Generate 1 buffer, put the resulting identifier in uvBuffer.
            uvBuffer = shaders["texture"].GetBuffer("VertexUV");
            uvAttribute = shaders["texture"].GetAttribute("VertexUV");

            // The following commands will talk about our 'uvBuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);

            // Give our vertices to OpenGL.
            IntPtr uvBufferSize = (IntPtr)(uv.Length * Vector2.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, uvBufferSize, uv, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(uvAttribute, 2, VertexAttribPointerType.Float, true, 0, 0);
        }

        public override void Update()
        {
            // TODO: A little inefficient
            Behaviour component = gameObject.GetComponent<Behaviour>();

            if (component != null)
            {
                component.Update();
            }

            gameObject.CalculateModelMatrix();
        }

        public override void Render()
        {
            Matrix4 modelViewProjectionMatrix = gameObject.ModelMatrix * (MainCamera.ViewMatrix * MainCamera.ProjectionMatrix);

            GL.UseProgram(shaders["texture"].ProgramId);

            // Bind our texture in Texture Unit 0
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            // Set our "textureSampler" sampler to user Texture Unit 0
            GL.Uniform1(textureSamplerUniform, TextureUnit.Texture0 - TextureUnit.Texture0);

            // Bind our texture in Texture Unit 1
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, textureId1);
            // Set our "textureSampler" sampler to user Texture Unit 1
            GL.Uniform1(textureSamplerUniform1, TextureUnit.Texture1 - TextureUnit.Texture0);

            // Bind the buffers.
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);

            // Load the model-view-projection matrix.
            GL.UniformMatrix4(mvpUniform, false, ref modelViewProjectionMatrix);

            // Draw the arrays.
            shaders["texture"].EnableVertexAttribArrays();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            shaders["texture"].DisableVertexAttribArrays();
        }
    }
}
