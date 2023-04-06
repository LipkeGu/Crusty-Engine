#type vertex
#version 430 core

in vec3 Position;
in vec2 TexCoord;
in vec3 Normal;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform vec3 Scale;

uniform float density;
uniform float gradient;
uniform vec3 lightPosition;

out vec2 passTexCoord;
out float visibility;
out vec4 testColor;
out vec3 surfaceNormal;
out vec3 toLightVector;
out vec3 toCameraVector;

void main()
{
    vec4 worldPosition = modelMatrix * vec4(Position, 1.0f);
    vec4 positionRelativeToCam = viewMatrix * worldPosition;
    gl_Position = projMatrix * positionRelativeToCam; 
    
    surfaceNormal = (modelMatrix * vec4(Normal, 0.0f)).xyz;
    toLightVector = lightPosition - worldPosition.xyz;
    toCameraVector = (inverse(viewMatrix) * vec4(0.0f, 0.0f, 0.0f, 1.0f)).xyz - worldPosition.xyz;

    float distance = length(positionRelativeToCam.xyz);
    visibility = exp(-pow((distance * density), gradient));    
    visibility = clamp(visibility, 0.0f, 1.0f);
        
    passTexCoord = vec2(Position.x * Scale.x, Position.z * Scale.z);
    testColor = vec4(Position.xyz,1) / 90;
}

#type fragment
#version 430 core

in vec2 passTexCoord;
in vec4 testColor;
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;
in float visibility;

uniform sampler2D uTexture;
uniform float AmbientStrength;
uniform vec3 lightColor;
uniform int EnableTestColor;
uniform vec3 fogColor;
uniform float shineDamper;
uniform float reflectivity;

out vec4 frag_colour;

void main()
{
    vec3 unitNormal = normalize(surfaceNormal);
    vec3 unitLightVector = normalize(toLightVector);
    vec3 unitVectorToCamera = normalize(toCameraVector);
    vec3 lightDirection = -unitLightVector;

    vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);

    float nDot1 = dot(unitNormal, unitLightVector);
    float brightness = max(nDot1, AmbientStrength);
    float specularFactor = dot(reflectedLightDirection, unitVectorToCamera);
    specularFactor = max(specularFactor, 0.0f);
    float dampedFactor = pow(specularFactor, shineDamper);

    vec3 finalSpecular = dampedFactor * reflectivity * lightColor;
    vec3 diffuse = brightness * lightColor;
    
    if (EnableTestColor == 1)
    {
	    diffuse = diffuse * testColor.xyz;
    }
    
    frag_colour = vec4(diffuse, 1.0f) * vec4(texture(uTexture, passTexCoord).xyz, 1.0f) + vec4(finalSpecular,1.0);
    frag_colour = mix(vec4(fogColor, 1.0f), frag_colour, visibility) ;
}
