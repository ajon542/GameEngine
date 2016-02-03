#version 430 core

in vec4 Color;

out vec4 FragColor;

uniform float LightIntensity;
uniform vec3 LightColor;
 
void
main()
{
    vec3 ambient = LightIntensity * LightColor;
	vec4 result = vec4(ambient, 1.0) * Color;
    FragColor = min(result, vec4(1.0));
}