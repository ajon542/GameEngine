#version 330

in vec3 VertexPosition;
in vec2 VertexUV;
out vec2 UV;

uniform mat4 MVPMatrix;

void main() 
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1);
    UV = VertexUV;
}
