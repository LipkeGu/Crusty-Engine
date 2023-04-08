using Crusty.Engine.System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;

namespace Crusty.Engine
{
	public class EngineLayer : GameWindow, IDisposable
	{
		public static CrustyEngine Engine { get; set; }

		private Vector2 lastPost;
		private double deltaTime = 0;
		bool hideCursor = true;
		bool firstMouse = true;
		FrameBuffer FrameBuffer;

		public static int GLVerMajor = 4;
		public static int GLVerMinor = 3;



		public EngineLayer(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
			int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
		{
			lastPost = new Vector2(width / 2, height / 2);
			Engine = new CrustyEngine();
		}

		public override void Dispose()
		{
			Engine.Dispose();
			FrameBuffer.Dispose();

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
				Engine.OnKeyDown(e.Key, e.Alt, e.Shift);

			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
			Engine.OnKeyUp(e.Key, e.Alt, e.Shift);
			base.OnKeyUp(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			Mouse.SetPosition(lastPost.X, lastPost.Y);

			GL.ClearColor(color: Color4.Black);
			Engine.Initialize(Width, Height);
			FrameBuffer = new FrameBuffer(Width, Height);

			base.OnLoad(e);
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
				Engine.OnMouseMove(new CursorPosition(deltaX, deltaY));
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			Engine.OnMouseUp(e.IsPressed, new CursorPosition(e.Position.X, e.Position.Y), e.Button);
			base.OnMouseUp(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			Engine.OnMouseDown(e.IsPressed, new CursorPosition(e.Position.X, e.Position.Y), e.Button);
			base.OnMouseDown(e);
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			deltaTime = e.Time;
			FrameBuffer.Render(Engine.Render, deltaTime);
			
			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnResize(EventArgs e)
		{
			FrameBuffer.OnResize(Width, Height);
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
			Engine.Update(e.Time);

			base.OnUpdateFrame(e);
		}
	}
}
