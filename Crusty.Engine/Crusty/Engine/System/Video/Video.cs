
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.System
{
	public class RendererState
	{
		public VSyncMode Vsync { get; private set; } = VSyncMode.Adaptive;

		public CullFaceMode CullFaceMode { get; private set; } = CullFaceMode.Back;

		public bool Culling { get; set; } = true;

		public bool Blending { get; set; } = true;

		public PolygonMode PolygonMode { get; set; } = PolygonMode.Fill;

		public int DepthRange { get; private set; } = 2;

		public RendererState() { }
	}


	public class Video
	{
		RendererState RendererState { get; set; } = new RendererState();

		public Video()
		{
			RendererState.Blending = GL.IsEnabled(EnableCap.Blend);
			RendererState.Culling = GL.IsEnabled(EnableCap.CullFace);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		}
	}
}
