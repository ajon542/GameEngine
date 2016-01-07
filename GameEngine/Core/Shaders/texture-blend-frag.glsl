#version 330

in vec2 UV;
out vec4 outputColor;
uniform sampler2D textureSampler;
uniform sampler2D textureSampler1;

void main() 
{
    vec3 texRGB = texture(textureSampler, UV).rgb;
    vec3 texRGB1 = texture(textureSampler1, UV).rgb;

    vec3 blend = mix(texRGB, texRGB1, 0.5);
    outputColor = vec4(blend, 1);
}