#version 330

in vec3 VertexNormal;
in vec3 VertexPosition;
in vec3 VertexColor;
in vec2 VertexUV;

out vec3 Normal;
out vec3 Position;
out vec4 Color;
out vec2 UV;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 MVPMatrix;

void
main()
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);

    // TODO: I think this normalMatrix is constructed from the ModelViewMatrix.
    mat3 normMatrix = transpose(inverse(mat3(ModelMatrix)));
    Normal = normMatrix * VertexNormal;
    Position = (ModelMatrix * vec4(VertexPosition, 1.0)).xyz;
    Color = vec4(VertexColor, 1.0);
	UV = VertexUV;
}