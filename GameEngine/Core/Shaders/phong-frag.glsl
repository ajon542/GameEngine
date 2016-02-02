#version 430 core

in VShaderOut
{
    vec3 Normal;
    vec3 Position;
	vec4 Color;
    vec2 UV;
} fShaderIn;

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
    vec3 n = normalize(fShaderIn.Normal);

    // Colors
	vec3 col = texture(mainTexture, fShaderIn.UV).rgb * fShaderIn.Color.rgb;
    vec4 texcolor = vec4(col, 1);
    vec4 light_ambient = LightAmbientIntensity * vec4(LightColor, 0.0);
    vec4 light_diffuse = LightDiffuseIntensity * vec4(LightColor, 0.0);

    // Ambient lighting
    FragColor = texcolor * light_ambient * vec4(MaterialAmbient, 0.0);

    // Diffuse lighting
    vec3 lightvec = normalize(LightPosition - fShaderIn.Position);
    float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);
    FragColor = FragColor + (light_diffuse * texcolor * vec4(MaterialDiffuse, 0.0)) * lambertmaterial_diffuse;

    // Specular lighting
    vec3 reflectionvec = normalize(reflect(-lightvec, fShaderIn.Normal));
    vec3 viewvec = normalize(vec3(inverse(ViewMatrix) * vec4(0,0,0,1)) - fShaderIn.Position); 
    float material_specularreflection = max(dot(fShaderIn.Normal, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), MaterialSpecExponent);
    FragColor = FragColor + vec4(MaterialSpecular * LightColor, 0.0) * material_specularreflection;
}
