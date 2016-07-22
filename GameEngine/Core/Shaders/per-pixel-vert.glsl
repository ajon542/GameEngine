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

uniform vec3 LightPos = vec3(10, 10, 0);

uniform vec4 _LightColor0 = vec4(1, 1, 1, 1);
 
// User-specified properties
uniform vec4 _Color = vec4(1, 1, 1, 1);     // Diffuse material color
uniform vec4 _SpecColor = vec4(1, 1, 1, 1); // Specular material color
uniform float _Shininess = 100.0f;           // Shininess

// Outputs to the fragment shader
out Fragment
{
    vec4 pos;
    vec4 posWorld;
    vec3 normalDir;
} fragment;

void main(void)
{
    fragment.posWorld = Object2World * vec4(position, 1);
    vec4 normalDir = vec4(normal, 0.0) * World2Object;
    fragment.normalDir = normalize(normalDir.xyz);
    fragment.pos = MATRIX_MVP * vec4(position, 1);
    gl_Position = fragment.pos;
}