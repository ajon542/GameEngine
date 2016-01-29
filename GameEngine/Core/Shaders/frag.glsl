#version 430 core

in VShaderOut
{
    vec4 Color;
} fShaderIn;

out vec4 FragColor;

void
main()
{
    FragColor = fShaderIn.Color;
}