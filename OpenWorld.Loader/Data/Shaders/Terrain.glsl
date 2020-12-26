#type vertex
#version 330 core
layout (location = 0) in vec3 Position;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform vec3 Scale;


out vec2 texCoord;

void main() {
	texCoord = vec2(Position.x * Scale.x, Position.z * Scale.z);
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(Position, 1.0f);
}

#type fragment
#version 330 core
out vec4 frag_colour;
in vec2 texCoord;

uniform sampler2D uTexture;
uniform float AmbientStrength;
uniform vec3 LightColor;

void main() {
	frag_colour = (texture(uTexture, texCoord) + vec4(LightColor, 1.0f)) * AmbientStrength;
}
