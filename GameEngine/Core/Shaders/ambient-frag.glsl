#version 430 core

in vec4 Color;

out vec4 FragColor;

uniform float LightIntensity;
uniform vec3 LightColor;
 
void
main()
{
    vec4 scatteredLight = vec4(LightColor, 1.0);
    FragColor = min(Color * scatteredLight * LightIntensity, vec4(1.0));
}