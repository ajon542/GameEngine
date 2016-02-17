#version 430 core

// Output
layout (location = 0) out vec4 color;

// Input from vertex shader
in Fragment
{
    vec3 N;
    vec3 L;
    vec3 V;
} fragment;

// Material properties
uniform vec3 diffuse_albedo = vec3(0.5, 0.2, 0.7);
uniform vec3 specular_albedo = vec3(0.7);
uniform float specular_power = 128.0;

void main(void)
{
    // Normalize the incoming N, L, and V vectors
    vec3 N = normalize(fragment.N);
    vec3 L = normalize(fragment.L);
    vec3 V = normalize(fragment.V);

    // Calculate R locally
    vec3 R = reflect(-L, N);

    // Uncomment for Blinn-Phong
    // Calculate the half vector, H
    //vec3 H = normalize(L + V);

    // Compute the diffuse and specular components for each fragment
    vec3 diffuse = max(dot(N, L), 0.0) * diffuse_albedo;
    vec3 specular = pow(max(dot(R, V), 0.0), specular_power) * specular_albedo;

    // Uncomment for Blinn-Phong
    // Replace the R.V calculation (as in Phong) with N.H
    //vec3 specular = pow(max(dot(N, H), 0.0), specular_power) * specular_albedo;

    // Write final color to the framebuffer
    color = vec4(diffuse + specular, 1.0);
}