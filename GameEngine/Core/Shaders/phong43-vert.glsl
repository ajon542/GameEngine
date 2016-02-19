#version 430 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

uniform mat4 mv_matrix;
uniform mat4 proj_matrix;

// Outputs to the fragment shader
out Fragment
{
    vec3 N;
    vec3 L;
    vec3 V;
} fragment;

// Position of light
uniform vec3 light_pos = vec3(100.0, 100.0, 100.0);

void main(void)
{
    // Calculate view-space coordinate
    vec4 P = mv_matrix * vec4(position, 1);

    // Calculate normal in view-space
    fragment.N = mat3(mv_matrix) * normal;

    // Calculate light vector
    vec3 light_viewspace = mat3(mv_matrix) * light_pos;
    fragment.L = light_viewspace - P.xyz;

    // Calculate view vector
    fragment.V = -P.xyz;

    // Calculate the clip-space position of each vertex
    gl_Position = proj_matrix * P;
}