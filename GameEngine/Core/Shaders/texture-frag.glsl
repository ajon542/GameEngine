#version 330

in vec2 UV;
out vec4 outputColor;
uniform sampler2D myTextureSampler;

void main() {
    outputColor = vec4(texture( myTextureSampler, UV ).rgb, 1);
}