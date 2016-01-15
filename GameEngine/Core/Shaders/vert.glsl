#version 330
 
in vec3 VertexPosition;
in vec3 VertexColor;
out vec4 Color;
uniform mat4 MVPMatrix;
 
void
main()
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);
 
    Color = vec4(VertexColor, 1.0);
}