#define INTELGL

using Crusty.Engine.Common.Camera;
using Crusty.Engine.Models;
using Crusty.Engine.UI;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Crusty.Engine
{
	public class EngineLayer : GameWindow, IDisposable
	{
		private ImGuiController ImGuiController;
		public static Dictionary<string, IControl> GUIWIndows = new Dictionary<string, IControl>();


		public static CrustyEngine Engine { get; set; }

		private Vector2 lastPost;
		private double deltaTime = 0;
		bool hideCursor = true;
		bool firstMouse = true;

#if !INTELGL
		FrameBuffer FrameBuffer;
#endif
		public static int GLVerMajor = 4;
		public static int GLVerMinor = 3;

		public EngineLayer(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
			int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
		{
			lastPost = new Vector2(Bounds.X + (ClientRectangle.Width / 2),
				Bounds.Y + (ClientRectangle.Height / 2));
			
			Engine = new CrustyEngine();
			CursorVisible = false;
			CursorGrabbed = true;
		}

		public override void Dispose()
		{
			Engine.Dispose();
#if !INTELGL
			FrameBuffer.Dispose();
#endif
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
			GL.ClearColor(color: Color4.Black);
			Engine.Initialize(Width, Height);

			var rect = this.ClientRectangle;

			Mouse.SetPosition(Bounds.X + (rect.Width / 2), Bounds.Y + (rect.Height / 2));

#if !INTELGL
			FrameBuffer = new FrameBuffer(Width, Height);
#endif
			ImGuiController = new ImGuiController(Width, Height);

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
#if !INTELGL
			FrameBuffer.Render(Engine.Render, deltaTime);
#else
			Engine.Render(deltaTime);
#endif

			foreach (var item in GUIWIndows.Values)
			{
				item.Draw();
			}

			ImGuiController.Render();

			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnResize(EventArgs e)
		{
			if (Width == 0  || Height == 0) return;
			GL.Viewport(new Rectangle(0, 0, Width, Height));

#if !INTELGL
			FrameBuffer.OnResize(Width, Height);
#endif
			Engine.OnResize(Width, Height);
			ImGuiController.WindowResized(Width, Height);
			base.OnResize(e);
		}

		protected override void OnUnload(EventArgs e)
		{
			Engine.OnUnload();

			base.OnUnload(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Engine.Update(e.Time);



            ImGuiController.Update(this, (float)e.Time);
			base.OnUpdateFrame(e);
		}
	}
}
