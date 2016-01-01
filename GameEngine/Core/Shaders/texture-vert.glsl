#version 330

in vec3 vPosition;
in vec2 vertexUV;
out vec2 UV;

uniform mat4 mvp;

void main() 
{
    gl_Position = mvp * vec4(vPosition, 1);
    UV = vertexUV;
}
