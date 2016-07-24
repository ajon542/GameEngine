#version 430 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

uniform mat4 MATRIX_MVP;
uniform mat4 MATRIX_MV;
uniform mat4 MATRIX_V;
uniform mat4 MATRIX_P;
uniform mat4 MATRIX_VP;
uniform mat4 Object2World;
uniform mat4 World2Object;
uniform vec3 WorldCameraPos;

// User-specified properties
uniform mat4 _SkyBoxMatrix_VP;

// Outputs to the fragment shader
out Fragment
{
    vec3 texCoords;
} fragment;

void main(void)
{
    gl_Position = _SkyBoxMatrix_VP * vec4(position, 1.0);
    fragment.texCoords = position;
}