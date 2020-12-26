#type vertex
#version 330 core
layout (location = 0) in vec3 position;

out vec3 pTexCoord;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main() {
	gl_Position = projMatrix * viewMatrix * vec4(position, 1.0f);
	pTexCoord = position;
}

#type fragment
#version 330 core
in vec3 pTexCoord;
out vec4 frag_colour;

uniform samplerCube uTexture;
uniform float AmbientStrength;
uniform vec3 LightColor;

void main() {

	frag_colour = (vec4(texture(uTexture, pTexCoord).xyz, 1.0f) + vec4(LightColor, 1.0f)) * AmbientStrength;
}
