#version 430 core

in vec3 VertexNormal;
in vec3 VertexPosition;
in vec3 VertexColor;

out VShaderOut
{
    vec3 Normal;
    vec3 Position;
    vec4 Color;
} vShaderOut;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 MVPMatrix;

void
main()
{
    gl_Position = MVPMatrix * vec4(VertexPosition, 1.0);

    // TODO: I think this normalMatrix is constructed from the ModelViewMatrix.
    mat3 normMatrix = transpose(inverse(mat3(ModelMatrix)));
    vShaderOut.Normal = normMatrix * VertexNormal;
    vShaderOut.Position = (ModelMatrix * vec4(VertexPosition, 1.0)).xyz;
    vShaderOut.Color = vec4(VertexColor, 1.0);
}