
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

		public void OnKeyDown(Key key)
		{
			switch (key)
			{
				case Key.F4:
					switch (RendererState.PolygonMode)
					{
						case PolygonMode.Point:
							RendererState.PolygonMode = PolygonMode.Line;
							break;
						case PolygonMode.Line:

							RendererState.PolygonMode = PolygonMode.Fill;
							break;
						case PolygonMode.Fill:

							RendererState.PolygonMode = PolygonMode.Point;
							break;
						default:
							break;
					}

					GL.PolygonMode(MaterialFace.FrontAndBack, RendererState.PolygonMode);
					break;
				case Key.F3:
					if (RendererState.Blending)
					{
						GL.Disable(EnableCap.Blend);
						RendererState.Blending = false;
					}
					else
					{
						GL.Enable(EnableCap.Blend);
						RendererState.Blending = true;
					}
					break;
				case Key.F2:
					if (RendererState.Culling)
					{
						GL.Disable(EnableCap.CullFace);
						RendererState.Culling = false;
					}
					else
					{
						GL.Enable(EnableCap.CullFace);
						RendererState.Culling = true;
					}
					break;
				default:
					break;
			}
		}
	}
}
