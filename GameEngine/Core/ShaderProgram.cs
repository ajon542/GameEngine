﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Core
{
    /// <summary>
    /// Class encapsulating common functionality for a shader program.
    /// </summary>
    public class ShaderProgram
    {
        /// <summary>
        /// Information about the shader program attributes.
        /// </summary>
        private class AttributeInfo
        {
            public string name = "";
            public int address = -1;
            public int size = 0;
            public ActiveAttribType type;
        }

        /// <summary>
        /// Information about the shader program uniforms.
        /// </summary>
        private class UniformInfo
        {
            public string name = "";
            public int address = -1;
            public int size = 0;
            public ActiveUniformType type;
        }

        public int ProgramID { get; private set; }
        private int vShaderID = -1;
        private int fShaderID = -1;
        private int attributeCount;
        private int uniformCount;

        private Dictionary<string, AttributeInfo> attributes = new Dictionary<string, AttributeInfo>();
        private Dictionary<string, UniformInfo> uniforms = new Dictionary<string, UniformInfo>();
        private Dictionary<string, uint> buffers = new Dictionary<string, uint>();

        /// <summary>
        /// Creates a new instance of the <see cref="ShaderProgram"/> class.
        /// </summary>
        /// <param name="vshader">The vertex shader.</param>
        /// <param name="fshader">The fragment shader.</param>
        /// <param name="fromFile">Specifies whether the shader is in a file or as a string.</param>
        public ShaderProgram(string vshader, string fshader, bool fromFile = false)
        {
            // Create the shader program.
            ProgramID = GL.CreateProgram();

            if (fromFile)
            {
                // Load the shaders from a file.
                LoadShaderFromFile(vshader, ShaderType.VertexShader);
                LoadShaderFromFile(fshader, ShaderType.FragmentShader);
            }
            else
            {
                // Load the shaders from a string.
                LoadShaderFromString(vshader, ShaderType.VertexShader);
                LoadShaderFromString(fshader, ShaderType.FragmentShader);
            }

            // Link the shaders.
            Link();

            // Generate the buffers.
            GenBuffers();
        }

        /// <summary>
        /// Load, compile and attach the shader object to the program object.
        /// </summary>
        /// <param name="code">String representation of shader code.</param>
        /// <param name="type">The type of shader.</param>
        /// <param name="address">The address of the shader object.</param>
        private void LoadShader(string code, ShaderType type, out int address)
        {
            address = GL.CreateShader(type);
            GL.ShaderSource(address, code);
            GL.CompileShader(address);
            GL.AttachShader(ProgramID, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        /// <summary>
        /// Load a shader from a string.
        /// </summary>
        /// <param name="code">String representation of shader code.</param>
        /// <param name="type">The type of shader.</param>
        public void LoadShaderFromString(string code, ShaderType type)
        {
            if (type == ShaderType.VertexShader)
            {
                LoadShader(code, type, out vShaderID);
            }
            else if (type == ShaderType.FragmentShader)
            {
                LoadShader(code, type, out fShaderID);
            }
        }

        /// <summary>
        /// Load a shader from a string.
        /// </summary>
        /// <param name="filename">Filename containing the shader code.</param>
        /// <param name="type">The type of shader.</param>
        public void LoadShaderFromFile(string filename, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                if (type == ShaderType.VertexShader)
                {
                    LoadShader(sr.ReadToEnd(), type, out vShaderID);
                }
                else if (type == ShaderType.FragmentShader)
                {
                    LoadShader(sr.ReadToEnd(), type, out fShaderID);
                }
            }
        }

        /// <summary>
        /// Link the shader program object.
        /// </summary>
        public void Link()
        {
            // Link the shader program object.
            GL.LinkProgram(ProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(ProgramID));

            // Get attribute and uniform count from the program object.
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveAttributes, out attributeCount);
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveUniforms, out uniformCount);

            // Get information about the active attributes.
            for (int i = 0; i < attributeCount; i++)
            {
                AttributeInfo info = new AttributeInfo();
                int length;

                StringBuilder name = new StringBuilder();

                GL.GetActiveAttrib(ProgramID, i, 256, out length, out info.size, out info.type, name);

                info.name = name.ToString();
                info.address = GL.GetAttribLocation(ProgramID, info.name);
                attributes.Add(name.ToString(), info);
            }

            // Get information about the active uniforms.
            for (int i = 0; i < uniformCount; i++)
            {
                UniformInfo info = new UniformInfo();
                int length;

                StringBuilder name = new StringBuilder();

                GL.GetActiveUniform(ProgramID, i, 256, out length, out info.size, out info.type, name);

                info.name = name.ToString();
                uniforms.Add(name.ToString(), info);
                info.address = GL.GetUniformLocation(ProgramID, info.name);
            }
        }

        /// <summary>
        /// Generate the buffer objects.
        /// </summary>
        public void GenBuffers()
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                uint buffer;
                GL.GenBuffers(1, out buffer);

                buffers.Add(attributes.Values.ElementAt(i).name, buffer);
            }

            for (int i = 0; i < uniforms.Count; i++)
            {
                uint buffer;
                GL.GenBuffers(1, out buffer);

                buffers.Add(uniforms.Values.ElementAt(i).name, buffer);
            }
        }

        /// <summary>
        /// Enable the vertex attribute arrays.
        /// </summary>
        public void EnableVertexAttribArrays()
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                GL.EnableVertexAttribArray(attributes.Values.ElementAt(i).address);
            }
        }

        /// <summary>
        /// Disable the vertex attribute arrays.
        /// </summary>
        public void DisableVertexAttribArrays()
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                GL.DisableVertexAttribArray(attributes.Values.ElementAt(i).address);
            }
        }

        /// <summary>
        /// Get the location of the attribute variable.
        /// </summary>
        /// <param name="name">The name of the attribute variable.</param>
        /// <returns>The location of the variable.</returns>
        public int GetAttribute(string name)
        {
            if (!attributes.ContainsKey(name))
            {
                throw new Exception("The attribute " + name + " does not exist");
            }
            return attributes[name].address;
        }

        /// <summary>
        /// Get the location of the uniform variable.
        /// </summary>
        /// <param name="name">The name of the uniform variable.</param>
        /// <returns>The location of the variable.</returns>
        public int GetUniform(string name)
        {
            if (!uniforms.ContainsKey(name))
            {
                throw new Exception("The uniform variable " + name + " does not exist");
            }
            return uniforms[name].address;
        }

        /// <summary>
        /// Get the location of the buffer object.
        /// </summary>
        /// <param name="name">The name of the buffer object.</param>
        /// <returns>The location of the buffer object.</returns>
        public uint GetBuffer(string name)
        {
            if (!buffers.ContainsKey(name))
            {
                throw new Exception("The buffer " + name + " does not exist");
            }
            return buffers[name];
        }
    }
}
