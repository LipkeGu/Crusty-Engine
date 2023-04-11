using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using SharpFont;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Crusty.Engine;

namespace OpenTK_example_5
{
	public class Text : IDisposable
	{
		FreeTypeFont font;
		Matrix4 Proj = Matrix4.Identity;
		Shader shader = new Shader();

		int Width = 0;
		int Height = 0;

		public Text()
		{
			font = new FreeTypeFont(32);
			shader.Create("Data/Shaders/Text.glsl");
		}

		public void Dispose()
		{
			shader.Dispose();
		}

		public void OnReSize(int width, int height)
		{
			Width = width; Height = height;

			Proj *= Matrix4.CreateScale(new Vector3(1f / this.Width, 1f / this.Height, 1.0f));
			Proj *= Matrix4.CreateOrthographicOffCenter(0.0f, Width, Height, 0.0f, -1.0f, 1.0f);
		}

		public void RenderText(string text)
		{
			shader.Use();
			shader.Set_Mat4("projMatrix", Proj);
			shader.Set_Vec3("textColor", new Vector3(1));

			font.RenderText(text, 25.0f, 50.0f, 1.2f, new Vector2(1f, 0f));
			shader.Unuse();
		}
	}

	public struct Character
	{
		public int TextureID { get; set; }
		public Vector2 Size { get; set; }
		public Vector2 Bearing { get; set; }
		public int Advance { get; set; }
	}

	public class FreeTypeFont
	{	Dictionary<uint, Character> _characters = new Dictionary<uint, Character>();
		int _vao;
		int _vbo;

		public FreeTypeFont(uint pixelheight)
		{
			var fs = new FileStream("Data/Fonts/FreeSans.ttf",FileMode.Open,FileAccess.Read);
			// initialize library
			var lib = new Library();

			//Face face = new Face(lib, "FreeSans.ottf");

			var assembly = Assembly.GetExecutingAssembly();
			//string[] names = assembly.GetManifestResourceNames();
		
			var ms = new MemoryStream();
			fs.CopyTo(ms);
			var face = new Face(lib, ms.ToArray(), 0);
			fs.Close();
			fs.Dispose();
			face.SetPixelSizes(0, pixelheight);

			// set 1 byte pixel alignment 
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

			// set texture unit
			GL.ActiveTexture(TextureUnit.Texture0);

			// Load first 128 characters of ASCII set
			for (uint c = 0; c < 128; c++)
			{
				try
				{
					// load glyph
					//face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
					face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
					var glyph = face.Glyph;
					var bitmap = glyph.Bitmap;

					// create glyph texture
					var texObj = GL.GenTexture();
					GL.BindTexture(TextureTarget.Texture2D, texObj);
					GL.TexImage2D(TextureTarget.Texture2D, 0,
								  PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
								  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

					// set texture parameters
					GL.TextureParameter(texObj, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.TextureParameter(texObj, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
					GL.TextureParameter(texObj, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
					GL.TextureParameter(texObj, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

					// add character
					Character ch = new Character
					{
						TextureID = texObj,
						Size = new Vector2(bitmap.Width, bitmap.Rows),
						Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop),
						Advance = (int)glyph.Advance.X.Value
					};

					_characters.Add(c, ch);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}

			// bind default texture
			GL.BindTexture(TextureTarget.Texture2D, 0);

			// set default (4 byte) pixel alignment 
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

			float[] vquad =
			{
            // x      y      u     v    
                0.0f, -1.0f,   0.0f, 0.0f,
				0.0f,  0.0f,   0.0f, 1.0f,
				1.0f,  0.0f,   1.0f, 1.0f,
				0.0f, -1.0f,   0.0f, 0.0f,
				1.0f,  0.0f,   1.0f, 1.0f,
				1.0f, -1.0f,   1.0f, 0.0f
			};

			// Create [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
			_vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, 4 * 6 * 4, vquad, BufferUsageHint.StaticDraw);

			// [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
			_vao = GL.GenVertexArray();
			GL.BindVertexArray(_vao);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * 4, 0);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * 4, 2 * 4);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public void RenderText(string text, float x, float y, float scale, Vector2 dir)
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindVertexArray(_vao);

			var angle_rad = (float)Math.Atan2(dir.Y, dir.X);
			var rotateM = Matrix4.CreateRotationZ(angle_rad);
			var transOriginM = Matrix4.CreateTranslation(new Vector3(x, y, 0f));

			// Iterate through all characters
			var char_x = 0.0f;
			foreach (var c in text)
			{
				if (!_characters.ContainsKey(c))
					continue;

				var ch = _characters[c];

				var	w = ch.Size.X * scale;
				var h = ch.Size.Y * scale;
				var xrel = char_x + ch.Bearing.X * scale;
				var yrel = (ch.Size.Y - ch.Bearing.Y) * scale;

				// Now advance cursors for next glyph (note that advance is number of 1/64 pixels)
				char_x += (ch.Advance >> 6) * scale; // Bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

				var scaleM = Matrix4.CreateScale(new Vector3(w, h, 1.0f));
				var transRelM = Matrix4.CreateTranslation(new Vector3(xrel, yrel, 0.0f));

				var modelM = scaleM * transRelM * rotateM * transOriginM; // OpenTK `*`-operator is reversed
				GL.UniformMatrix4(0, false, ref modelM);




				// Render glyph texture over quad
				GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

				// Render quad
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
			}

			GL.BindVertexArray(0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
	}
}
