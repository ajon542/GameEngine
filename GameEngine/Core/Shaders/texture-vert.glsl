#version 330

// Input vertex data, different for all executions of this shader.
in vec3 vPosition;
in vec2 vertexUV;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

// Values that stay constant for the whole mesh.
uniform mat4 modelview;

void main() {

    // Output position of the vertex, in clip space : MVP * position
    gl_Position =  modelview * vec4(vPosition, 1);

    // UV of the vertex. No special space for this one.
    UV = vertexUV;
}
