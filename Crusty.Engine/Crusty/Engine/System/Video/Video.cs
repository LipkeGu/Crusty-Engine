
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine
{
	public class RendererState
	{
		public VSyncMode Vsync { get; private set; } = VSyncMode.Adaptive;

		public CullFaceMode CullFaceMode { get; private set; } = CullFaceMode.Back;

		public bool Culling { get; set; } = true;


		public System.Numerics.Vector4 ClearColor = new System.Numerics.Vector4();
		
		public bool Blending { get; set; } = true;

		public PolygonMode PolygonMode { get; set; } = PolygonMode.Fill;

		public int DepthRange { get; private set; } = 2;

		public RendererState() { }
	}


	public class Video
	{
		public RendererState RendererState { get; set; } = new RendererState();

		public Video()
		{
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			RendererState.Blending = GL.IsEnabled(EnableCap.Blend);
			RendererState.Culling = GL.IsEnabled(EnableCap.CullFace);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(RendererState.CullFaceMode);
		}

		public void BeginRender()
		{
			GL.ClearColor(RendererState.ClearColor.X, RendererState.ClearColor.Y, 
				RendererState.ClearColor.Z, RendererState.ClearColor.W);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

		}
	}
}
