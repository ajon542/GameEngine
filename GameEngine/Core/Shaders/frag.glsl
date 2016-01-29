#version 430 core

in VShaderOut
{
    vec4 Color;
} FShaderIn;

out vec4 FragColor;

void
main()
{
    FragColor = FShaderIn.Color;
}