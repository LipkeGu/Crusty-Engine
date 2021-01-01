using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace OpenWorld.Engine.Video
{
	public class Texture : IDisposable
	{
		public int Id { get; private set; } = 0;

		private TextureTarget textureTarget;

		public Texture(List<string> filenames)
		{
			Id = GL.GenTexture();
			textureTarget = TextureTarget.TextureCubeMap;

			for (var i = 0; i < filenames.Count; i++)
				if (!System.IO.File.Exists(filenames[i]))
				{
					Console.WriteLine("[E] Could not find Texture: {0}", filenames[i]);
					return;
				}

			CreateCube(filenames);
		}

		public Texture(string filename)
		{
			Id = GL.GenTexture();
			textureTarget = TextureTarget.Texture2D;

			if (!System.IO.File.Exists(filename))
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
			using (var bitmap = new System.Drawing.Bitmap(filename))
			{
				var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly, bitmap.PixelFormat);

				__setTextureFormat(index, ref data);

				bitmap.UnlockBits(data);
			}
		}

		private void __setTextureFormat(int index, ref BitmapData data)
		{
			GL.TextureParameter(Id, TextureParameterName.TextureMinFilter, (int)All.Nearest);
			GL.TextureParameter(Id, TextureParameterName.TextureMagFilter, (int)All.Nearest);
			GL.TextureParameter(Id, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TextureParameter(Id, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			
			var GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
			var InternalPixelFormat = PixelInternalFormat.Rgba;

			switch (data.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgr;
					InternalPixelFormat = PixelInternalFormat.Rgb;
					break;
				default:
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					InternalPixelFormat = PixelInternalFormat.Rgba;
					GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
					break;
			}

			switch (textureTarget)
			{
				case TextureTarget.TextureCubeMap:
					GL.TextureParameter(Id, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
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
			switch (textureTarget)
			{
				case TextureTarget.TextureCubeMap:
					GL.Enable(EnableCap.TextureCubeMap);
					break;
				case TextureTarget.Texture2D:
				default:
					GL.Enable(EnableCap.Texture2D);
					break;
			}

			GL.ActiveTexture(TextureUnit.Texture0 + slot);
			GL.BindTexture(textureTarget, Id);
		}

		public void UnBind()
		{
			GL.BindTexture(textureTarget, 0);

			switch (textureTarget)
			{
				case TextureTarget.TextureCubeMap:
					GL.Disable(EnableCap.TextureCubeMap);
					break;
				case TextureTarget.Texture2D:
				default:
					GL.Disable(EnableCap.Texture2D);
					break;
			}
		}

		public void CleanUp()
		{
			UnBind();
			GL.DeleteTexture(1);
		}

		public void Dispose()
		{
			this.UnBind();
		}
	}
}
