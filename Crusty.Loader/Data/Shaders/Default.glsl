#type vertex
#version ##GL_VERSION##0 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TexCoord;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform sampler2D uTexture;

out vec4 pColor;

void main() {
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(Position.xyz, 1.0f);
}

#type fragment
#version ##GL_VERSION##0 core
out vec4 frag_colour;
in vec4 pColor;

void main() {
	frag_colour = vec4(1.0f, 0.0f, 0.0f, 1.0f);
}
