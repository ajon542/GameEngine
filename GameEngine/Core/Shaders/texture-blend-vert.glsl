#version 430 core

in vec3 VertexPosition;
in vec2 VertexUV;

out VShaderOut
{
    vec2 UV;
} vShaderOut;

uniform mat4 MVPMatrix;

void main() 
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1);
    vShaderOut.UV = VertexUV;
}
