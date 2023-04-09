using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.System
{
	public class FrameBuffer : IDisposable
	{
		public int Width { get; private set; }
		public int Height { get; private set; }

		public int Id { get; private set; }
		private int rbid = 0;
		public int texId = 0;

		IShader screenShader = new Shader();
		VertexArray screen = new VertexArray();

		private TextureTarget textureTarget = TextureTarget.Texture2D;

		public void BindTexture()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(textureTarget, texId);
		}

		public void UnBindTexture()
		{
			GL.BindTexture(textureTarget, 0);
		}

		public void Render(Action<double> delegatedDrawFunction, double delaTime)
		{
			#region "Render Scene to Texture"
			Bind();
			delegatedDrawFunction(delaTime);
			UnBind();
			#endregion

			#region "Draw Texture to Window"
			GL.Clear(ClearBufferMask.ColorBufferBit);
			BindTexture();
			screen.Draw(screenShader);
			UnBindTexture();
			#endregion
		}

		public FrameBuffer(int width, int height)
		{
			{
				var _id = 0;
				GL.GenFramebuffers(1, out _id);
				Id = _id;
			}

			Create();
			OnResize(width, height);
		}

		private void Create()
		{
			screenShader.Create("Data/Shaders/FrameBuffer.glsl");
			screen.Create();

			var Vertices = new List<Vector2>
			{
				new Vector2(-1.0f, 1.0f),
				new Vector2(-1.0f, -1.0f),
				new Vector2(1.0f, -1.0f),

				new Vector2(-1.0f, 1.0f),
				new Vector2(1.0f, -1.0f),
				new Vector2(1.0f, 1.0f)
			};

			screen.Upload(Vertices);

			var texCoords = new List<Vector2>
			{
				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f),

				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 1.0f)
			};

			screen.Upload(texCoords);
		}

		public void OnResize(int width, int height)
		{
			Width = width;
			Height = height;

			GL.Viewport(0, 0, Width, Height);
			
			CreateTexture();
		}

		private void CreateTexture()
		{
			Bind();

			GL.GenTextures(1, out texId);
			GL.BindTexture(textureTarget, texId);

			GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba8, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
			GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)All.Nearest);
			GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)All.Nearest);

			GL.BindTexture(textureTarget, 0);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texId, 0);

			GL.GenRenderbuffers(1, out rbid);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbid);

			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Width, Height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment,
				RenderbufferTarget.Renderbuffer, rbid);

			if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
				throw new Exception("[E] FrameBuffer: Not complete!");

			UnBind();
		}

		public void Bind()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);

			if (rbid != 0)
				GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbid);
		}

		public void UnBind()
		{
			if (rbid != 0)
				GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}

		public void Dispose()
		{
			GL.DeleteFramebuffer(Id);
			GL.DeleteRenderbuffer(rbid);
			GL.DeleteTexture(texId);
		}
	}
}
