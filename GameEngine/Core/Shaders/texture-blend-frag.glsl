#version 330

in vec2 UV;
out vec4 outputColor;
uniform sampler2D textureSampler;

void main() 
{
    outputColor = vec4(texture(textureSampler, UV).rgb, 1);
}