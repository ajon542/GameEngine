#version 430 core

in Fragment
{
    vec4 Color;
} fragment;

out vec4 FragColor;

void main(void)
{
    FragColor = fragment.Color;
}  