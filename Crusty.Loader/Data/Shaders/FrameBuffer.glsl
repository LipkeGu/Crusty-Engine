#type vertex
#version ##GL_VERSION##0 core

layout(location = 0) in vec2 Position;
layout(location = 1) in vec2 TexCoord;

out vec2 passTexcoord;

void main()
{
	passTexcoord = TexCoord;
	gl_Position = vec4(Position, 0.0f, 1.0f);
}

#type fragment
#version ##GL_VERSION##0 core

in vec2 passTexcoord;
out vec4 outColor;

uniform sampler2D texFramebuffer;

void main()
{
	outColor = texture(texFramebuffer, passTexcoord);
}
