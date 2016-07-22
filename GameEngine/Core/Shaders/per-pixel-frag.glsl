#version 430 core

// Output
layout (location = 0) out vec4 color;

uniform mat4 MATRIX_MVP;
uniform mat4 MATRIX_MV;
uniform mat4 MATRIX_V;
uniform mat4 MATRIX_P;
uniform mat4 MATRIX_VP;
uniform mat4 Object2World;
uniform mat4 World2Object;
uniform vec3 WorldCameraPos;

uniform vec3 LightPosition = vec3(10, 0, 0);
uniform vec4 LightColor = vec4(1, 1, 1, 1);

// User-specified properties
uniform vec4 _Color = vec4(1, 1, 1, 1);     // Diffuse material color
uniform vec4 _SpecColor = vec4(1, 1, 1, 1); // Specular material color
uniform float _Shininess = 100.0f;           // Shininess

// Input from vertex shader
in Fragment
{
    vec4 pos;
    vec4 posWorld;
    vec3 normalDir;
} fragment;


void main(void)
{
    vec3 normalDirection = normalize(fragment.normalDir);
    vec3 viewDirection = normalize(WorldCameraPos - fragment.posWorld.xyz);

    vec3 lightDirection;
    float attenuation;

    attenuation = 1.0; // no attenuation
    lightDirection = normalize(LightPosition);

    // Calculate the ambient light.
    vec4 lightmodel_ambient = vec4(0.1, 0.1, 0.1, 1);
    vec3 ambientLighting = lightmodel_ambient.rgb * _Color.rgb;

    // Calculate the diffuse light.
    vec3 diffuseReflection = 
        attenuation * LightColor.rgb * _Color.rgb
        * max(0.0, dot(normalDirection, lightDirection));

    // Calculate the specular light.
    vec3 specularReflection;

    if (dot(normalDirection, lightDirection) < 0.0) 
    {
        // Light source on the wrong side? No specular reflection.
        specularReflection = vec3(0.0, 0.0, 0.0);
    }
    else
    {
        // Light source on the correct side. 
        vec3 reflectDirection = reflect(-lightDirection, normalDirection);
        specularReflection = 
            attenuation *
            LightColor.rgb *
            _SpecColor.rgb *
            pow(max(0.0, dot(reflectDirection, viewDirection)), _Shininess);
    }

    // Write final color to the framebuffer
    color = vec4(ambientLighting + diffuseReflection + specularReflection, 1.0);
}