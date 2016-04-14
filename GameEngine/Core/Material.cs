using System;
using System.Collections.Generic;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameEngine.Core
{
    public class Material
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float SpecularExponent { get; set; }

        private Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();

        public Material()
        {
            Ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
            SpecularExponent = 2.0f;
        }

        public void SetShaders(string vShader, string fShader)
        {
            shaders.Add("default", new ShaderProgram(vShader, fShader, true));
        }

        public void UseProgram()
        {
            GL.UseProgram(shaders["default"].ProgramId);
        }

        public void EnableVertexAttribArrays()
        {
            shaders["default"].EnableVertexAttribArrays();
        }

        public void DisableVertexAttribArrays()
        {
            shaders["default"].DisableVertexAttribArrays();
        }

        public uint GetBuffer(string name)
        {
            return shaders["default"].GetBuffer(name);
        }

        public int GetAttribute(string name)
        {
            return shaders["default"].GetAttribute(name);
        }

        public int GetUniform(string name)
        {
            return shaders["default"].GetUniform(name);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            GL.UniformMatrix4(shaders["default"].GetUniform(name), false, ref matrix);
        }
    }
}
