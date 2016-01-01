using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
    // TODO: This texture example needs to be cleaned up.
    // TODO: Find somewhere to put the sample assets. For now they are in the GameSpecific/Assets/Textures
    class TextureExample : Scene
    {
        /// <summary>
        /// Vertex array object identifier.
        /// </summary>
        private uint vertexArrayId;

        /// <summary>
        /// Vertex buffer identifier.
        /// </summary>
        private uint vertexBuffer;

        /// <summary>
        /// Vertex attribute identifier.
        /// </summary>
        private int vertexAttribute;

        /// <summary>
        /// Vertex buffer data.
        /// </summary>
        private float[] vertexBufferData =
        {
            -1.0f, -1.0f, -5.0f,
             1.0f, -1.0f, -5.0f,
             1.0f,  1.0f, -5.0f,

             1.0f,  1.0f, -5.0f,
            -1.0f,  1.0f, -5.0f,
            -1.0f, -1.0f, -5.0f,
        };

        /// <summary>
        /// UV buffer identifier.
        /// </summary>
        private uint uvBuffer;

        /// <summary>
        /// UV attribute identifier.
        /// </summary>
        private int uvAttribute;

        /// <summary>
        /// UV buffer data.
        /// </summary>
        private float[] uvData = 
        {
            1, 0,
            1, 1,
            0, 1,
            0, 1,
            0, 0,
            1, 0,
        };

        // Get a handle for our "myTextureSampler" uniform
        int textureUniform;
        int textureId;

        GameObject gameObject = new GameObject();

        // Storage for the shader programs.
        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public override void Initialize()
        {
            // Add default shaders.
            shaders.Add("texture", new ShaderProgram("Core/Shaders/texture-vert.glsl", "Core/Shaders/texture-frag.glsl", true));

            // Load the texture.
            textureId = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/Nuclear-Symbol.bmp");

            // Generate a VAO and set it as the current one.
            GL.GenVertexArrays(1, out vertexArrayId);
            GL.BindVertexArray(vertexArrayId);

            // Texture id.
            textureUniform = shaders["texture"].GetUniform("myTextureSampler");

            // Vertices.

            // Generate 1 buffer, put the resulting identifier in vertexbuffer.
            vertexBuffer = shaders["texture"].GetBuffer("vPosition");
            vertexAttribute = shaders["texture"].GetAttribute("vPosition");

            // The following commands will talk about our 'vertexbuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            // Give our vertices to OpenGL.
            IntPtr vertexBufferSize = (IntPtr)(vertexBufferData.Length * Vector3.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferSize, vertexBufferData, BufferUsageHint.StaticDraw);

            // UVs.

            // Generate 1 buffer, put the resulting identifier in uvBuffer.
            uvBuffer = shaders["texture"].GetBuffer("vertexUV");
            uvAttribute = shaders["texture"].GetAttribute("vertexUV");

            // The following commands will talk about our 'uvBuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);

            // Give our vertices to OpenGL.
            IntPtr uvBufferSize = (IntPtr)(uvData.Length * Vector2.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, uvBufferSize, uvData, BufferUsageHint.StaticDraw);
        }

        public override void Update()
        {
            gameObject.CalculateModelMatrix();
            gameObject.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1, 1200 / (float)800, 1.0f, 1000.0f);
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["texture"].ProgramId);

            // Bind our texture in Texture Unit 0
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            // Set our "myTextureSampler" sampler to user Texture Unit 0
            GL.Uniform1(textureUniform, TextureUnit.Texture0 - TextureUnit.Texture0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.VertexAttribPointer(vertexAttribute, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.VertexAttribPointer(uvAttribute, 2, VertexAttribPointerType.Float, true, 0, 0);

            GL.UniformMatrix4(shaders["texture"].GetUniform("modelview"), false, ref gameObject.ModelViewProjectionMatrix);

            shaders["texture"].EnableVertexAttribArrays();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            shaders["texture"].DisableVertexAttribArrays();
        }
    }
}
