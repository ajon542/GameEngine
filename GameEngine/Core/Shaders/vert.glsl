#version 430 core
 
in vec3 VertexPosition;
in vec3 VertexColor;

out VShaderOut
{
    vec4 Color;
} vShaderOut;

uniform mat4 MVPMatrix;

void
main()
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);
 
    vShaderOut.Color = vec4(VertexColor, 1.0);
}