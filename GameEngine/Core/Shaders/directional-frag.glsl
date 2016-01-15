#version 330

uniform vec4 Ambient;
in vec4 Color;
out vec4 FragColor;
 
void
main()
{
    vec4 scatteredLight = Ambient;
    FragColor = min(Color * scatteredLight, vec4(1.0));
}