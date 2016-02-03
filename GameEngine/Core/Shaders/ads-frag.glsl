#version 430 core

in vec4 Color;
in vec3 Position;
in vec3 Normal;

out vec4 FragColor;

uniform float LightAmbientIntensity;
uniform float LightDiffuseIntensity;
uniform vec3 LightColor;
uniform vec3 LightDirection;

uniform vec3 EyeWorldPos;
uniform float SpecularIntensity;
uniform float SpecularPower;

void
main()
{
    vec4 ambientColor = vec4(LightColor * LightAmbientIntensity, 1.0f);
    vec3 lightDirection = -LightDirection;
    vec3 normal = normalize(Normal);

    float diffuseFactor = dot(normal, lightDirection);

    vec4 diffuseColor = vec4(0, 0, 0, 0);
    vec4 specularColor = vec4(0, 0, 0, 0);

    if (diffuseFactor > 0) {
        diffuseColor = vec4(LightColor, 1.0f) * LightDiffuseIntensity * diffuseFactor;

        vec3 vertexToEye = normalize(EyeWorldPos - Position);
        vec3 lightReflect = normalize(reflect(LightDirection, normal));
        float specularFactor = dot(vertexToEye, lightReflect);
        if (specularFactor > 0) {
            specularFactor = pow(specularFactor, SpecularPower);
            specularColor = vec4(LightColor * SpecularIntensity * specularFactor, 1.0f);
        }
    }

    FragColor = Color * (ambientColor + diffuseColor + specularColor);
}