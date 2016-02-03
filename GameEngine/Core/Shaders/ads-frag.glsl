#version 430 core

in vec4 Color;
in vec3 Normal;

out vec4 FragColor;

uniform float LightAmbientIntensity;
uniform float LightDiffuseIntensity;
uniform vec3 LightColor;
uniform vec3 LightDirection;

void
main()
{
    vec4 ambientColor = vec4(LightColor * LightAmbientIntensity, 1.0);

    float diffuseFactor = dot(normalize(Normal), -LightDirection);

    vec4 diffuseColor;

    if (diffuseFactor > 0) {
        diffuseColor = vec4(LightColor * LightDiffuseIntensity * diffuseFactor, 1.0f);
    }
    else {
        diffuseColor = vec4(0, 0, 0, 0);
    }

    FragColor = Color * (ambientColor + diffuseColor);
}