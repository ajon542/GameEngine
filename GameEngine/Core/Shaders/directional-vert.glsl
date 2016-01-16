#version 330

in vec3 VertexNormal;
in vec3 VertexPosition;
in vec3 VertexColor;

out vec4 Color;
out vec3 Normal;

uniform mat4 MVPMatrix;
uniform mat4 NormalMatrix;
 
void
main()
{
    mat3 normalMatrix = mat3(NormalMatrix);
    Color = vec4(VertexColor, 1.0);
    Normal = normalize(normalMatrix * VertexNormal);
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);
}