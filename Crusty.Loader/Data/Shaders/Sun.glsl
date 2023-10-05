#type vertex
#version ##GL_VERSION##0 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TexCoords;
layout(location = 2) in vec3 Normal;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main() {
	vec4 worldPosition = modelMatrix * vec4(Position, 1.0f);
	vec4 positionRelativeToCam = viewMatrix * worldPosition;
	gl_Position = projMatrix * positionRelativeToCam;
}

#type fragment
#version ##GL_VERSION##0 core

out vec4 frag_colour;

void main() {
	frag_colour = vec4(1.0f, 1.0f, 1.0f, 1.0f);
}
