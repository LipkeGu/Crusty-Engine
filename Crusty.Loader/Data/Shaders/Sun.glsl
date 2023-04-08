#type vertex
#version 430 core

in vec3 Position;
in vec2 TexCoords;
in vec3 Normal;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main() {
	vec4 worldPosition = modelMatrix * vec4(Position, 1.0f);
    vec4 positionRelativeToCam = viewMatrix * worldPosition;
    gl_Position = projMatrix * positionRelativeToCam;
}

#type fragment
#version 430 core

out vec4 frag_colour;

void main() {
    frag_colour = vec4(1.0f, 1.0f, 1.0f, 1.0f);
}
