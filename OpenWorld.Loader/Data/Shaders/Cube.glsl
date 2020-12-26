#type vertex
#version 330 core
layout (location = 0) in vec3 Position;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main() {
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(Position, 1.0f);
}

#type fragment
#version 330 core
out vec4 frag_colour;

void main() {
	frag_colour = vec4(1.0f, 1.0f, 0.0f, 1.0f);
}
