#version 330

uniform vec4 Ambient; // sets lighting level, same across many vertices
in vec4 color;
out vec4 outputColor;
 
void
main()
{
    vec4 scatteredLight = Ambient; // this is the only light
    // modulate surface color with light, but saturate at white
    outputColor = min(color * scatteredLight, vec4(1.0));
}