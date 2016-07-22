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

uniform vec3 LightPos;

uniform vec4 _LightColor0;
 
// User-specified properties
uniform vec4 _Color;
uniform vec4 _SpecColor;
uniform float _Shininess;

// Input from vertex shader
in Fragment
{
    vec4 pos;
    vec4 posWorld;
    vec3 normalDir;
} fragment;


void main(void)
{
    vec3 normalDirection = normalize(fragment.normalDir);
    vec3 viewDirection = normalize(WorldCameraPos - fragment.posWorld.xyz);

    vec3 lightDirection;
    float attenuation;

    attenuation = 1.0; // no attenuation
    lightDirection = normalize(LightPos);

    // Write final color to the framebuffer
    color = vec4(1, 1, 1, 1);
}