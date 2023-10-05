#type vertex
#version ##GL_VERSION##0 core

layout(location = 0) in vec2 Position;
layout(location = 1) in vec2 TexCoord;

uniform mat4 projMatrix;

out vec2 pTexCoord;

void main() {
	gl_Position = projMatrix * vec4(Position, 0.0f , 1.0f);
	pTexCoord = TexCoord;
}

#type fragment
#version ##GL_VERSION##0 core
in vec2 pTexCoord;
out vec4 frag_colour;
uniform sampler2D uTexture;
uniform vec3 textColor;

void main()
{
    vec2 uv = pTexCoord;
    float text = texture(uTexture, uv).r;
    frag_colour = vec4(textColor.rgb * text, text);
}
