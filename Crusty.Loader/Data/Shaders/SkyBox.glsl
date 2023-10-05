#type vertex
#version ##GL_VERSION##0 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TexCoords;
layout(location = 2) in vec3 Normal;

out vec3 passTexCoord;
out vec3 surfaceNormal;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

uniform vec3 lightPosition[4];
out vec3 toLightVector[4];
out vec3 toCameraVector;

void main() {
	vec4 worldPosition = modelMatrix * vec4(Position, 1.0f);
	vec4 positionRelativeToCam = viewMatrix * worldPosition;
	gl_Position = projMatrix * positionRelativeToCam;

	surfaceNormal = (modelMatrix * vec4(Normal, 0.0f)).xyz;
	
	for (int i=0;i<4;i++)
	{
		toLightVector[i] = lightPosition[i] - worldPosition.xyz;
	}
	
	toCameraVector = (inverse(viewMatrix) * vec4(0.0f, 0.0f, 0.0f, 1.0f)).xyz - worldPosition.xyz;

	passTexCoord = Position;
}

#type fragment
#version ##GL_VERSION##0 core
in vec3 passTexCoord;
in vec3 surfaceNormal;
in vec3 toLightVector[4];
in vec3 toCameraVector;

out vec4 frag_colour;

uniform samplerCube uTexture;
uniform float AmbientStrength;
uniform vec3 lightColor[4];
uniform vec3 attenuation[4];
uniform float shineDamper;
uniform float reflectivity;

void main() {
	vec3 unitNormal = normalize(surfaceNormal);
	vec3 unitVectorToCamera = normalize(toCameraVector);
	
	vec3 totalDiffuse = vec3(0.0f);
	vec3 totalSpecular = vec3(0.0f);
	
	for (int i = 0; i< 4; i++)
	{
		 float distance = length(toLightVector[i]);
		 //float attFactor = attenuation[i].x + (attenuation[i].y * distance) + (attenuation[i].z * distance * distance);
		 vec3 unitLightVector = normalize(toLightVector[i]);
		 vec3 lightDirection = -unitLightVector;
		 vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);
		 float nDot1 = dot(unitNormal, unitLightVector);
		 float brightness = max(nDot1, 0.0f);
		 float specularFactor = dot(reflectedLightDirection, unitVectorToCamera);
		 specularFactor = max(specularFactor, 0.0f);
		 float dampedFactor = pow(specularFactor, shineDamper);
		 totalDiffuse = totalDiffuse + (brightness * lightColor[i]);
		 totalSpecular = totalSpecular + (dampedFactor * reflectivity * lightColor[i]);
	 }
	
	totalDiffuse = max(totalDiffuse, 0.2f);

	frag_colour = vec4(totalDiffuse, 1.0f) * vec4(texture(uTexture, passTexCoord).xyz, 1.0f) + vec4(totalSpecular,1.0);
}
