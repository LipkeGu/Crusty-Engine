using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Crusty.Engine
{
	public class Texture : IDisposable
	{
		public int Id { get; private set; } = 0;

		private TextureTarget textureTarget;

		public float LODBias { get; private set; } = -0.4f; 

		public Texture(List<string> filenames)
		{
			var _id = 0;
			GL.GenTextures(1, out _id);
			Id = _id;

			textureTarget = TextureTarget.TextureCubeMap;

			for (var i = 0; i < filenames.Count; i++)
				if (!File.Exists(filenames[i]))
				{
					Console.WriteLine("[E] Could not find Texture: {0}", filenames[i]);
					return;
				}

			CreateCube(filenames);
		}

		public Texture(string filename)
		{
			var _id = 0;
			GL.GenTextures(1, out _id);
			Id = _id;

			textureTarget = TextureTarget.Texture2D;

			if (!File.Exists(filename))
			{
				Console.WriteLine("[E] Could not find Texture: {0}", filename);
				return;
			}

			Bind();

			Create(filename);

			UnBind();
		}

		private void CreateCube(List<string> filenames)
		{
			Bind();

			for (var i = 0; i < filenames.Count; i++)
				Create(filenames[i], TextureTarget.TextureCubeMap, i);

			UnBind();
		}

		private void Create(string filename, TextureTarget textureTarget = TextureTarget.Texture2D, int index = 0)
		{
			using (var bitmap = new Bitmap(filename))
			{
				var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly, bitmap.PixelFormat);

				__setTextureFormat(index, ref data);

				bitmap.UnlockBits(data);
			}
		}

		private void __setTextureFormat(int index, ref BitmapData data)
		{
			int texMinFilterValue = (int)All.Nearest;
			int texMagFilterValue = (int)All.Nearest;

			switch (textureTarget)
			{
				case TextureTarget.TextureCubeMap:
					texMinFilterValue = (int)All.LinearMipmapLinear;
					texMagFilterValue = (int)All.LinearMipmapLinear;
					break;
				case TextureTarget.Texture2D:
				default:
					texMinFilterValue = (int)All.Linear;
					texMagFilterValue = (int)All.Linear;
					break;
			}

			GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, texMinFilterValue);
			GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, texMagFilterValue);
			GL.TexParameter(textureTarget, TextureParameterName.TextureLodBias, LODBias);
			GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			var GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
			var InternalPixelFormat = PixelInternalFormat.Rgba;

			switch (data.PixelFormat)
			{
				case PixelFormat.Format24bppRgb:
					GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgr;
					InternalPixelFormat = PixelInternalFormat.Rgb;
					break;
				default:
				case PixelFormat.Format32bppArgb:
					InternalPixelFormat = PixelInternalFormat.Rgba;
					GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
					break;
			}

			switch (textureTarget)
			{
				case TextureTarget.TextureCubeMap:
					GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
					GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + index, 0, InternalPixelFormat, data.Width, data.Height,
						0, GLPixelFormat, PixelType.UnsignedByte, data.Scan0);
					GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
					break;
				default:
				case TextureTarget.Texture2D:
					GL.TexImage2D(textureTarget, 0, InternalPixelFormat, data.Width, data.Height,
						0, GLPixelFormat, PixelType.UnsignedByte, data.Scan0);
					GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
					break;
			}
		}

		public void Bind(int slot = 0)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + slot);
			GL.BindTexture(textureTarget, Id);
		}

		public void UnBind()
		{
			GL.BindTexture(textureTarget, 0);
		}

		public void CleanUp()
		{
			UnBind();
			GL.DeleteTexture(1);
		}

		public void Dispose()
		{
			CleanUp();
		}
	}
}
