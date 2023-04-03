#type vertex
#version 330 core

in vec3 Position;
in vec2 TexCoord;
in vec3 Nomal;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform vec3 Scale;

uniform float density;
uniform float gradient;

out vec2 texCoord;
out float visibility;
out vec4 testColor;

void main()
{
    vec4 worldPosition = modelMatrix * vec4(Position, 1.0f);
    vec4 positionRelativeToCam = viewMatrix * worldPosition;
    gl_Position = projMatrix * positionRelativeToCam; 
    
    float distance = length(positionRelativeToCam.xyz);
    visibility = exp(-pow((distance * density), gradient));    
    visibility = clamp(visibility, 0.0f, 1.0f);
        
    texCoord = vec2(Position.x * Scale.x, Position.z * Scale.z);
    testColor = vec4(Position.xyz,1) / 90;
}

#type fragment
#version 330 core
out vec4 frag_colour;
in vec2 texCoord;
in vec4 testColor;

in float visibility;


uniform sampler2D uTexture;
uniform float AmbientStrength;
uniform vec3 LightColor;
uniform int EnableTestColor;
uniform vec3 fogColor;

void main()
{
 vec4 lColor = vec4(LightColor * AmbientStrength, 1.0f);
 if (EnableTestColor == 1)
 {
    lColor = lColor * testColor;
 }
 
 frag_colour = vec4(texture(uTexture, texCoord).xyz, 1.0f) * lColor;
 frag_colour = mix(vec4(fogColor, 1.0f), frag_colour, visibility);
 
}
