#version 430 core

layout (location = 0) in vec4 VertexPosition;
layout (location = 1) in vec4 InstanceColor;
layout (location = 2) in vec4 InstancePosition;

out Fragment
{
    vec4 Color;
} fragment;

uniform mat4 MVPMatrix;

void main(void)
{
    gl_Position = MVPMatrix * (VertexPosition + InstancePosition);
    fragment.Color = InstanceColor;
}