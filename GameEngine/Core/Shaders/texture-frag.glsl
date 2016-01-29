#version 430 core

in VShaderOut
{
    vec2 UV;
} fShaderIn;

out vec4 FragColor;
uniform sampler2D textureSampler;

void main() 
{
    FragColor = vec4(texture(textureSampler, fShaderIn.UV).rgb, 1);
}