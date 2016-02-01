﻿using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core.Graphics
{
    public static class Vao
    {
        // TODO: Fix these, they don't belong in the batch class.
        public const int Cube = 0;
        public const int Cone = 1;
        public const int Count = 2;
    }

    public static class Vbo
    {
        public const int Vertices = 0;
        public const int Colours = 1;
        public const int Elements = 2;
        public const int Count = 3;
    }

    // TODO: Improve the batch class, it isn't actualyl batching anything :)
    // In order to improve this class we are going to need to remove the shader specific
    // stuff from the class.
    // A shader is most often going to have vertices, colours, normals and texture coords.
    // The vertices need to be passed to the shader so we should extend the Vbo enum to
    // account for all the types.
    // Currently, the ShaderProgram class handles the GenBuffer, Attribute and Uniform
    // locations. This may present a problem when trying to separate the shader and batch.
    // The usage we are looking for is:
    // - Specify a shader
    // - Render batch
    // We currently have:
    // - Render batch with hardcoded shader

    class Batch
    {
        private PrimitiveType renderType;

        private uint[] vertexArrObjects;
        private uint[] vertexBufObjects;

        private int indicesCount;

        public Batch(Mesh mesh)
        {
            renderType = mesh.RenderType;

            vertexArrObjects = new uint[Vao.Count];
            vertexBufObjects = new uint[Vbo.Count];

            GL.GenVertexArrays(Vao.Count, vertexArrObjects);
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.GenBuffers(Vbo.Count, vertexBufObjects);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufObjects[Vbo.Vertices]);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufObjects[Vbo.Colours]);
            GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
            GL.ColorPointer(3, ColorPointerType.Float, 0, 0);
            GL.EnableClientState(ArrayCap.ColorArray);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vertexBufObjects[Vbo.Elements]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            GL.BindVertexArray(vertexArrObjects[Vao.Cube]);
            GL.DrawElements(renderType, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
    }

    class ShaderBatch
    {
        private PrimitiveType renderType;

        private uint vertexArrObject;
        private int indicesCount;
        private uint positionBuffer;
        private int positionAttr;
        private uint colourBuffer;
        private int colourAttr;
        private uint normalBuffer;
        private int normalAttr;
        private uint elementBuffer;
        private int uvAttr;
        private uint uvBuffer;

        private int textureId;

        private Mesh mesh;

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public ShaderBatch(Mesh mesh)
        {
            this.mesh = mesh;
            renderType = mesh.RenderType;

            // TODO: Supply 1x1 pixel texture in the case where a texture not supplied.
            // See: http://stackoverflow.com/questions/14978986/find-out-if-gl-texture-2d-is-active-in-shader
            // Supply either uv coords or basic colors.
            if (mesh.UV.Count != 0)
            {
                shaders.Add("default", new ShaderProgram("Core/Shaders/phong-tex-vert.glsl", "Core/Shaders/phong-tex-frag.glsl", true));
                textureId = Texture.LoadTexture("Core/GameSpecific/Assets/Textures/Chrome.png");
                uvBuffer = shaders["default"].GetBuffer("VertexUV");
                uvAttr = shaders["default"].GetAttribute("VertexUV");
            }
            else
            {
                shaders.Add("default", new ShaderProgram("Core/Shaders/phong-vert.glsl", "Core/Shaders/phong-frag.glsl", true));
                colourBuffer = shaders["default"].GetBuffer("VertexColor");
                colourAttr = shaders["default"].GetAttribute("VertexColor");
            }

            positionBuffer = shaders["default"].GetBuffer("VertexPosition");
            positionAttr = shaders["default"].GetAttribute("VertexPosition");
            normalBuffer = shaders["default"].GetBuffer("VertexNormal");
            normalAttr = shaders["default"].GetAttribute("VertexNormal");

            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);

            Vector3[] vertices = mesh.Vertices.ToArray();
            Vector3[] colours = mesh.Colours.ToArray();
            Vector3[] normals = mesh.Normals.ToArray();
            Vector2[] uv = mesh.UV.ToArray();
            int[] indices = mesh.Indices.ToArray();

            IntPtr verticesLength = (IntPtr)(vertices.Length * Vector3.SizeInBytes);
            IntPtr coloursLength = (IntPtr)(colours.Length * Vector3.SizeInBytes);
            IntPtr normalsLength = (IntPtr)(normals.Length * Vector3.SizeInBytes);
            IntPtr uvsLength = (IntPtr)(uv.Length * Vector2.SizeInBytes);
            IntPtr indicesLength = (IntPtr)(indices.Length * sizeof(int));
            indicesCount = indices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesLength, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Supply either uv coords or basic colors.
            if (mesh.UV.Count != 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, uvsLength, uv, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(uvAttr, 2, VertexAttribPointerType.Float, false, 0, 0);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, colourBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, coloursLength, colours, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(colourAttr, 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normalsLength, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(normalAttr, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesLength, indices, BufferUsageHint.StaticDraw);
        }

        public void Render(Matrix4 modelViewProjectionMatrix, Matrix4 viewMatrix, Matrix4 modelMatrix, Light activeLight)
        {
            GL.UseProgram(shaders["default"].ProgramId);

            if (mesh.UV.Count != 0)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.Uniform1(shaders["default"].GetUniform("mainTexture"), TextureUnit.Texture0 - TextureUnit.Texture0);
            }

            GL.BindVertexArray(vertexArrObject);

            shaders["default"].EnableVertexAttribArrays();

            Vector3 ambientColor = new Vector3(0.1880f, 0.1880f, 0.1880f);
            Vector3 diffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 specularColor = new Vector3(1.0f, 1.0f, 1.0f);
            float specularExponent = 10.0f;

            GL.UniformMatrix4(shaders["default"].GetUniform("MVPMatrix"), false, ref modelViewProjectionMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ViewMatrix"), false, ref viewMatrix);
            GL.UniformMatrix4(shaders["default"].GetUniform("ModelMatrix"), false, ref modelMatrix);
            GL.Uniform3(shaders["default"].GetUniform("MaterialAmbient"), ref ambientColor);
            GL.Uniform3(shaders["default"].GetUniform("MaterialDiffuse"), ref diffuseColor);
            GL.Uniform3(shaders["default"].GetUniform("MaterialSpecular"), ref specularColor);
            GL.Uniform1(shaders["default"].GetUniform("MaterialSpecExponent"), specularExponent);
            GL.Uniform3(shaders["default"].GetUniform("LightPosition"), ref activeLight.Position);
            GL.Uniform3(shaders["default"].GetUniform("LightColor"), ref activeLight.Color);
            GL.Uniform1(shaders["default"].GetUniform("LightDiffuseIntensity"), activeLight.DiffuseIntensity);
            GL.Uniform1(shaders["default"].GetUniform("LightAmbientIntensity"), activeLight.AmbientIntensity);

            GL.DrawElements(mesh.RenderType, indicesCount, DrawElementsType.UnsignedInt, 0);

            shaders["default"].DisableVertexAttribArrays();
        }
    }
}
