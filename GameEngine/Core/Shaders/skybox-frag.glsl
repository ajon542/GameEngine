#version 430 core

// Output
layout (location = 0) out vec4 color;

uniform mat4 MATRIX_MVP;
uniform mat4 MATRIX_MV;
uniform mat4 MATRIX_V;
uniform mat4 MATRIX_P;
uniform mat4 MATRIX_VP;
uniform mat4 Object2World;
uniform mat4 World2Object;
uniform vec3 WorldCameraPos;

uniform vec3 LightPosition = vec3(10, 0, 0);
uniform vec4 LightColor = vec4(1, 1, 1, 1);

uniform samplerCube skybox;

// Input from vertex shader
in Fragment
{
    vec3 texCoords;
} fragment;


void main(void)
{
    color = texture(skybox, fragment.texCoords);
}