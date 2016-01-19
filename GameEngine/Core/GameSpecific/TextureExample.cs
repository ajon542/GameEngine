using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngine.Core.Graphics;

namespace GameEngine.Core.GameSpecific
{
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
        /// UV buffer identifier.
        /// </summary>
        private uint uvBuffer;

        /// <summary>
        /// UV attribute identifier.
        /// </summary>
        private int uvAttribute;

        /// <summary>
        /// Texture sampler uniform handle in fragment shader.
        /// </summary>
        private int textureSamplerUniform;

        /// <summary>
        /// Loaded texture identifier.
        /// </summary>
        private int textureId;

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
            shaders.Add("texture", new ShaderProgram("Core/Shaders/texture-vert.glsl", "Core/Shaders/texture-frag.glsl", true));

            // Load the texture.
            textureId = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/UV-Template.bmp");

            // TODO: Figure out what a vertex array is used for.
            // This question highlights my concern as everything works without this even being set.
            // http://stackoverflow.com/questions/5970087/understanding-vertex-array-objects-glgenvertexarrays
            // The following document appears to be an explanation:
            // http://www.openglsuperbible.com/2013/12/09/vertex-array-performance/

            // Generate a VAO and set it as the current one.
            GL.GenVertexArrays(1, out vertexArrayId);
            GL.BindVertexArray(vertexArrayId);

            // Get the texture sampler uniform location from the fragment shader.
            textureSamplerUniform = shaders["texture"].GetUniform("textureSampler");

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

            // UVs.

            // Generate 1 buffer, put the resulting identifier in uvBuffer.
            uvBuffer = shaders["texture"].GetBuffer("VertexUV");
            uvAttribute = shaders["texture"].GetAttribute("VertexUV");

            // The following commands will talk about our 'uvBuffer' buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);

            // Give our vertices to OpenGL.
            IntPtr uvBufferSize = (IntPtr)(uv.Length * Vector2.SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, uvBufferSize, uv, BufferUsageHint.StaticDraw);
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
            gameObject.ViewProjectionMatrix = MainCamera.ViewMatrix * MainCamera.ProjectionMatrix;
            gameObject.ModelViewProjectionMatrix = gameObject.ModelMatrix * gameObject.ViewProjectionMatrix;
        }

        public override void Render()
        {
            GL.UseProgram(shaders["texture"].ProgramId);

            // Bind our texture in Texture Unit 0
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            // Set our "textureSampler" sampler to user Texture Unit 0
            GL.Uniform1(textureSamplerUniform, TextureUnit.Texture0 - TextureUnit.Texture0);

            // Set the vertex buffer attribute pointer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.VertexAttribPointer(vertexAttribute, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Set the UV buffer attribute pointer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.VertexAttribPointer(uvAttribute, 2, VertexAttribPointerType.Float, true, 0, 0);

            // Load the model-view-projection matrix.
            GL.UniformMatrix4(mvpUniform, false, ref gameObject.ModelViewProjectionMatrix);

            // Draw the arrays.
            shaders["texture"].EnableVertexAttribArrays();

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            shaders["texture"].DisableVertexAttribArrays();
        }
    }
}
