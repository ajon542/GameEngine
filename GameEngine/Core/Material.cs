using System;
using System.Collections.Generic;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using NLog;

namespace GameEngine.Core
{
    public class Material
    {
        public string Name { get; private set; }

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private uint vertexArrObject;
        private uint elementBuffer;

        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float SpecularExponent { get; set; }

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public Material()
        {
            throw new NotImplementedException("this ctor should be removed, can't use a material without a shader");
        }

        public Material(string name, string vShader, string fShader)
        {
            Name = name;
            logger.Log(LogLevel.Info, Name);

            shaders.Add("default", new ShaderProgram(vShader, fShader, true));
            Ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
            SpecularExponent = 2.0f;
        }

        public void Initialize()
        {
            logger.Log(LogLevel.Info, "");

            // Send OpenGL our vertex data.
            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);
        }

        public void UseProgram()
        {
            GL.UseProgram(shaders["default"].ProgramId);
        }

        public void BindVertexArray()
        {
            GL.BindVertexArray(vertexArrObject);
        }

        public void EnableVertexAttribArrays()
        {
            shaders["default"].EnableVertexAttribArrays();
        }

        public void DisableVertexAttribArrays()
        {
            shaders["default"].DisableVertexAttribArrays();
        }

        public bool GetBuffer(string name, out uint buffer)
        {
            return shaders["default"].GetBuffer(name, out buffer);
        }

        public bool GetAttribute(string name, out int attribute)
        {
            return shaders["default"].GetAttribute(name, out attribute);
        }

        public bool GetUniform(string name, out int uniform)
        {
            return shaders["default"].GetUniform(name, out uniform);
        }

        public void SetVector3(string name, Vector3 vector)
        {
            int uniform;
            if (shaders["default"].GetUniform(name, out uniform))
            {
                GL.Uniform3(uniform, ref vector);
            }
        }

        public void SetFloat(string name, float value)
        {
            int uniform;
            if (shaders["default"].GetUniform(name, out uniform))
            {
                GL.Uniform1(uniform, value);
            }
        }

        public void SetVector4(string name, Vector4 vector)
        {
            int uniform;
            if (shaders["default"].GetUniform(name, out uniform))
            {
                GL.Uniform4(uniform, ref vector);
            }
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int uniform;
            if (shaders["default"].GetUniform(name, out uniform))
            {
                GL.UniformMatrix4(uniform, false, ref matrix);
            }
        }

        public void SetPositionBuffer(Vector3[] buffer)
        {
            uint bufferId;
            int attributeId;

            GetBuffer("position", out bufferId);
            GetAttribute("position", out attributeId);

            IntPtr bufferLength = (IntPtr)(buffer.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);


            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void SetNormalBuffer(Vector3[] buffer)
        {
            uint bufferId;
            int attributeId;

            GetBuffer("normal", out bufferId);
            GetAttribute("normal", out attributeId);

            IntPtr bufferLength = (IntPtr)(buffer.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void SetElementBuffer(int[] buffer)
        {
            IntPtr bufferLength = (IntPtr)(buffer.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Cleanup any material resources.
        /// </summary>
        public void Destroy()
        {
            logger.Log(LogLevel.Info, "");

            foreach (KeyValuePair<string, ShaderProgram> kv in shaders)
            {
                kv.Value.Destroy();
            }

            GL.DeleteVertexArray(vertexArrObject);
            GL.DeleteBuffer(elementBuffer);
        }
    }
}
