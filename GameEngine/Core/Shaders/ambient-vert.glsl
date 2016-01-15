#version 330
 
// These attributes are what the stream must provide in order to render
// with this shader. i.e. it must provide a vector of positions and colors.
// All of these arrays must have the same number of elements.
in vec3 vPosition;
in vec3 vColor;
out vec4 color;
uniform mat4 mvp;
 
void
main()
{
    gl_Position = mvp * vec4(vPosition, 1.0);
 
    color = vec4(vColor, 1.0);
}