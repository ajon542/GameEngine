#version 430 core

in VShaderOut
{
    vec2 UV;
} fShaderIn;

out vec4 FragColor;
uniform sampler2D textureSampler;
uniform sampler2D textureSampler1;

void main() 
{
    vec3 texRGB = texture(textureSampler, fShaderIn.UV).rgb;
    vec3 texRGB1 = texture(textureSampler1, fShaderIn.UV).rgb;

    vec3 blend = mix(texRGB, texRGB1, 0.5);
    FragColor = vec4(blend, 1);
}