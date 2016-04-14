﻿using System;
using System.Collections.Generic;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameEngine.Core
{
    public class Material
    {
        private uint vertexArrObject;
        private uint elementBuffer;

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

        public void Initialize()
        {
            // Send OpenGL our vertex data.
            GL.GenVertexArrays(1, out vertexArrObject);
            GL.BindVertexArray(vertexArrObject);
            GL.GenBuffers(1, out elementBuffer);
        }

        public void SetShaders(string vShader, string fShader)
        {
            shaders.Add("default", new ShaderProgram(vShader, fShader, true));
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

        public void SetPositionBuffer(Vector3[] buffer)
        {
            IntPtr bufferLength = (IntPtr)(buffer.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, GetBuffer("position"));
            GL.BufferData(BufferTarget.ArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(GetAttribute("position"), 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void SetNormalBuffer(Vector3[] buffer)
        {
            IntPtr bufferLength = (IntPtr)(buffer.Length * Vector3.SizeInBytes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, GetBuffer("normal"));
            GL.BufferData(BufferTarget.ArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(GetAttribute("normal"), 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void SetElementBuffer(int[] buffer)
        {
            IntPtr bufferLength = (IntPtr)(buffer.Length * sizeof(int));
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, bufferLength, buffer, BufferUsageHint.StaticDraw);
        }
    }
}
