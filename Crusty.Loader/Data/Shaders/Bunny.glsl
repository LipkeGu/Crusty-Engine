#type vertex
#version ##GL_VERSION##0 core
const int MAX_MODELS = 128;

in vec3 Position;
in vec2 TexCoords;
in vec3 Normals;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix[MAX_MODELS];

void main() {
	gl_Position = projMatrix * viewMatrix * modelMatrix[gl_InstanceID] * vec4(Position, 1.0f);
}

#type fragment
#version ##GL_VERSION##0 core
out vec4 frag_colour;

void main() {
	frag_colour = vec4(1.0f, 1.0f, 0.0f, 1.0f);
}
