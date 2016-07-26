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

uniform sampler2D textureSampler1;
uniform sampler2D textureSampler2;

in Fragment
{
    vec2 texcoords;
} fragment;

void main() 
{
    vec3 texRGB1 = texture(textureSampler1, fragment.texcoords).rgb;
    vec3 texRGB2 = texture(textureSampler2, fragment.texcoords).rgb;

    vec3 blend = mix(texRGB1, texRGB2, 0.5);
    color = vec4(blend, 1);
}