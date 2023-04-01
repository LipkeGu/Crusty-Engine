using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;

namespace OpenWorld.Engine
{
	public class EngineLayer : GameWindow
	{
		public static OpenWorldEngine Engine { get; set; }

		private Vector2 lastPost = new Vector2(400.0f);
		private double deltaTime = 0;
		bool hideCursor = true;
		bool firstMouse = true;

		public EngineLayer(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
			int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
		{
			lastPost = new Vector2(width / 2, Height / 2);

			Engine = new OpenWorldEngine();
		}

		public override void Dispose()
		{
			Engine.Dispose();

			base.Dispose();
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				Exit();
			else if (e.Key == Key.F10)
			{
				if (hideCursor)
				{
					base.CursorGrabbed = false;
					base.CursorVisible = true;
					hideCursor = false;
				}
				else
				{
					hideCursor = true;
					base.CursorVisible = false;
					base.CursorGrabbed = true;
				}
			}
			else
				Engine.OnKeyDown(e.Key, (float)deltaTime);

			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			Mouse.SetPosition(lastPost.X, lastPost.Y);

			GL.ClearColor(color: Color4.CornflowerBlue);
			Engine.Initialize(Width, Height);

			base.OnLoad(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (firstMouse)
			{
				lastPost = new Vector2(e.Mouse.X, e.Mouse.Y);
				firstMouse = false;
			}
			else
			{
				var deltaX = e.Mouse.X - lastPost.X;
				var deltaY = e.Mouse.Y - lastPost.Y;

				lastPost = new Vector2(e.Mouse.X, e.Mouse.Y);
				Engine.OnMouseMove(deltaX, deltaY, deltaTime);
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);

		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			deltaTime = e.Time;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			Engine.Render(deltaTime);
			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnResize(EventArgs e)
		{
			Engine.OnResize(Width, Height);

			base.OnResize(e);
		}

		protected override void OnUnload(EventArgs e)
		{
			Engine.Unload();

			base.OnUnload(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Engine.Update(Width, Height, e.Time);

			base.OnUpdateFrame(e);
		}
	}
}
