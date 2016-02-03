#version 430 core

in vec3 VertexPosition;
in vec3 VertexNormal;
in vec3 VertexColor;

out vec4 Color;
out vec3 Position;
out vec3 Normal;

uniform mat4 MVPMatrix;
uniform mat4 ModelMatrix;

void
main()
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);

    mat3 normMatrix = transpose(inverse(mat3(ModelMatrix)));
    Normal = normMatrix * VertexNormal;
    Position = (ModelMatrix * vec4(VertexPosition, 1.0)).xyz;
    Color = vec4(VertexColor, 1.0);
}