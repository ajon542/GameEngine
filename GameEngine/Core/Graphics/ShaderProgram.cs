using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameEngine.Core.Debugging;
using OpenTK.Graphics.OpenGL;
using NLog;

namespace GameEngine.Core.Graphics
{
    /// <summary>
    /// Class encapsulating common functionality for a shader program.
    /// </summary>
    public class ShaderProgram
    {
        /// <summary>
        /// Reference to the logging mechanism.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        public int ProgramId { get; private set; }
        private int vShaderId = -1;
        private int fShaderId = -1;
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
            logger.Log(LogLevel.Info, "Creating shader programs vs={0} fs={1}", vshader, fshader);

            // Create the shader program.
            ProgramId = GL.CreateProgram();

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

            logger.Log(LogLevel.Info, "Shader program creation complete");
        }

        /// <summary>
        /// Load, compile and attach the shader object to the program object.
        /// </summary>
        /// <param name="code">String representation of shader code.</param>
        /// <param name="type">The type of shader.</param>
        /// <param name="address">The address of the shader object.</param>
        private void LoadShader(string code, ShaderType type, out int address)
        {
            logger.Log(LogLevel.Info, "Loading shader type={0}", type);

            address = GL.CreateShader(type);
            GL.ShaderSource(address, code);
            GL.CompileShader(address);

            // TODO: Decide what we are going to do here if an error occurs on one shader only.
            // Do we clean up all the shaders in this program?
            int statusCode = 0;
            GL.GetShader(address, ShaderParameter.CompileStatus, out statusCode);

            if (statusCode != 1)
            {
                GL.DeleteShader(address);
                GL.DeleteProgram(ProgramId);
                throw new GameEngineException("Compiling {0} failed with error code {1}", type, statusCode);
            }

            GL.AttachShader(ProgramId, address);
            GL.DeleteShader(address);
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
                LoadShader(code, type, out vShaderId);
            }
            else if (type == ShaderType.FragmentShader)
            {
                LoadShader(code, type, out fShaderId);
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
                    LoadShader(sr.ReadToEnd(), type, out vShaderId);
                }
                else if (type == ShaderType.FragmentShader)
                {
                    LoadShader(sr.ReadToEnd(), type, out fShaderId);
                }
            }
        }

        /// <summary>
        /// Link the shader program object.
        /// </summary>
        public void Link()
        {
            logger.Log(LogLevel.Info, "Linking shader");

            // Link the shader program object.
            GL.LinkProgram(ProgramId);

            // Check for errors during program linking.
            int statusCode = 0;
            GL.GetProgram(ProgramId, GetProgramParameterName.LinkStatus, out statusCode);

            if (statusCode != 1)
            {
                string programLog = GL.GetProgramInfoLog(ProgramId);
                GL.DeleteProgram(ProgramId);
                throw new GameEngineException(programLog);
            }

            // Get attribute and uniform count from the program object.
            GL.GetProgram(ProgramId, GetProgramParameterName.ActiveAttributes, out attributeCount);
            GL.GetProgram(ProgramId, GetProgramParameterName.ActiveUniforms, out uniformCount);

            // Get information about the active attributes.
            for (int i = 0; i < attributeCount; i++)
            {
                AttributeInfo info = new AttributeInfo();
                int length;
                int size;
                ActiveAttribType type;

                // See: https://github.com/opentk/opentk/issues/57
                StringBuilder name = new StringBuilder(32);

                GL.GetActiveAttrib(ProgramId, i, 256, out length, out size, out type, name);

                info.size = size;
                info.name = name.ToString();
                info.type = type;
                info.address = GL.GetAttribLocation(ProgramId, info.name);

                logger.Log(LogLevel.Info, "Att {0} location {1}", info.name, info.address);

                attributes.Add(name.ToString(), info);
            }

            // Get information about the active uniforms.
            for (int i = 0; i < uniformCount; i++)
            {
                UniformInfo info = new UniformInfo();
                int length;
                int size;
                ActiveUniformType type;

                // See: https://github.com/opentk/opentk/issues/57
                StringBuilder name = new StringBuilder(32);

                GL.GetActiveUniform(ProgramId, i, 256, out length, out size, out type, name);

                info.size = size;
                info.name = name.ToString();
                info.type = type;
                info.address = GL.GetUniformLocation(ProgramId, info.name);

                logger.Log(LogLevel.Info, "Uni {0} location {1}", info.name, info.address);

                uniforms.Add(name.ToString(), info);
            }
        }

        /// <summary>
        /// Generate the buffer objects.
        /// </summary>
        public void GenBuffers()
        {
            logger.Log(LogLevel.Info, "Generating buffers");

            for (int i = 0; i < attributes.Count; i++)
            {
                uint buffer;
                GL.GenBuffers(1, out buffer);

                buffers.Add(attributes.Values.ElementAt(i).name, buffer);

                logger.Log(LogLevel.Info, "Att {0} location {1}", attributes.Values.ElementAt(i).name, buffer);
            }

            for (int i = 0; i < uniforms.Count; i++)
            {
                uint buffer;
                GL.GenBuffers(1, out buffer);

                buffers.Add(uniforms.Values.ElementAt(i).name, buffer);

                logger.Log(LogLevel.Info, "Uni {0} location {1}", uniforms.Values.ElementAt(i).name, buffer);
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
        public bool GetAttribute(string name, out int attribute)
        {
            if (!attributes.ContainsKey(name))
            {
                attribute = -1;
                return false;
            }
            attribute = attributes[name].address;
            return true;
        }

        /// <summary>
        /// Get the location of the uniform variable.
        /// </summary>
        /// <param name="name">The name of the uniform variable.</param>
        /// <returns>The location of the variable.</returns>
        public bool GetUniform(string name, out int uniform)
        {
            if (!uniforms.ContainsKey(name))
            {
                uniform = -1;
                return false;
            }
            uniform = uniforms[name].address;
            return true;
        }

        /// <summary>
        /// Get the location of the buffer object.
        /// </summary>
        /// <param name="name">The name of the buffer object.</param>
        /// <returns>The location of the buffer object.</returns>
        public bool GetBuffer(string name, out uint buffer)
        {
            if (!buffers.ContainsKey(name))
            {
                buffer = 0;
                return false;
            }
            buffer = buffers[name];
            return true;
        }

        /// <summary>
        /// Cleanup the unmanaged shader resources.
        /// </summary>
        /// <remarks>
        /// IDisposable cannot be used since the GC is performed on a different
        /// thread to what the OpenGL context is on. Not entirely sure if there
        /// is a way around this.
        /// </remarks>
        public void Destroy()
        {
            // http://www.opentk.com/node/3693
            foreach (KeyValuePair<string, uint> kv in buffers)
            {
                logger.Log(LogLevel.Info, "Deleting buffer {0} location {1}", kv.Key, kv.Value);
                uint bufferId = kv.Value;
                GL.DeleteBuffers(1, ref bufferId);
            }

            GL.DeleteShader(vShaderId);
            GL.DeleteShader(fShaderId);
            GL.DeleteProgram(ProgramId);
        }
    }
}
