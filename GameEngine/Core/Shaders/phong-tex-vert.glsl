﻿#version 430 core

in vec3 VertexNormal;
in vec3 VertexPosition;
in vec2 VertexUV;

out VShaderOut
{
    vec3 Normal;
    vec3 Position;
    vec2 UV;
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
    vShaderOut.UV = VertexUV;
}