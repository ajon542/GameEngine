#version 430 core

// Output
layout (location = 0) out vec4 color;

// Input from vertex shader
in Fragment
{
    vec3 color;
} fragment;

void main(void)
{
    // Write incoming color to the framebuffer
    color = vec4(fragment.color, 1.0);
}