#version 430

in vec3 Normal;
in vec3 Position;
in vec4 Color;
in vec2 UV;
out vec4 FragColor;

uniform mat4 ViewMatrix;

uniform vec3 MaterialAmbient;
uniform vec3 MaterialDiffuse;
uniform vec3 MaterialSpecular;
uniform float MaterialSpecExponent;

uniform vec3 LightPosition;
uniform vec3 LightColor;
uniform float LightAmbientIntensity;
uniform float LightDiffuseIntensity;

uniform sampler2D mainTexture;

void
main()
{
    vec3 n = normalize(Normal);

    // Colors
    vec4 texcolor = vec4(texture(mainTexture, UV).rgb, 1);
    vec4 light_ambient = LightAmbientIntensity * vec4(LightColor, 0.0);
    vec4 light_diffuse = LightDiffuseIntensity * vec4(LightColor, 0.0);

    // Ambient lighting
    FragColor = texcolor * light_ambient * vec4(MaterialAmbient, 0.0);

    // Diffuse lighting
    vec3 lightvec = normalize(LightPosition - Position);
    float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);
    FragColor = FragColor + (light_diffuse * texcolor * vec4(MaterialDiffuse, 0.0)) * lambertmaterial_diffuse;

    // Specular lighting
    vec3 reflectionvec = normalize(reflect(-lightvec, Normal));
    vec3 viewvec = normalize(vec3(inverse(ViewMatrix) * vec4(0,0,0,1)) - Position); 
    float material_specularreflection = max(dot(Normal, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), MaterialSpecExponent);
    FragColor = FragColor + vec4(MaterialSpecular * LightColor, 0.0) * material_specularreflection;
}
