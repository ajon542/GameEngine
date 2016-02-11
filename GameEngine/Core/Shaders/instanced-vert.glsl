#version 430 core

layout (location = 0) in vec4 VertexPosition;
layout (location = 1) in vec4 InstanceColor;
layout (location = 2) in vec4 InstancePosition;

out Fragment
{
    vec4 Color;
} fragment;

void main(void)
{
    gl_Position = (VertexPosition + InstancePosition) * vec4(0.25, 0.25, 1.0, 1.0);
    fragment.Color = InstanceColor;
}