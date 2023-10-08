using ImGuiNET;
using Crusty.Engine.UI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

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

		FrameBuffer FrameBuffer;

		public static int GLVerMajor = 4;
		public static int GLVerMinor = 3;

		private void openGL_GUI_Content()
		{
			ImGui.LabelText(GL.GetString(StringName.Renderer), "Renderer: ");
			ImGui.LabelText(GL.GetString(StringName.Vendor), "Vendor: ");
			ImGui.LabelText(GL.GetString(StringName.Version), "Version: ");
			ImGui.LabelText(string.Format("{0}x{1}", Width, Height), "Auflösung: ");
		}

		public EngineLayer(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
			int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
		{
			lastPost = new Vector2(Bounds.X + (ClientRectangle.Width / 2),
				Bounds.Y + (ClientRectangle.Height / 2));
			CursorVisible = false;
			CursorGrabbed = true;

			GUIWIndows.Add("main0", new Window("OpenGL Info", openGL_GUI_Content));
			GUIWIndows.Add("main1", new Window("Crusty Engine", () =>
			{
				var mRay = Engine.Camera.RayPosition;
				ImGui.LabelText(string.Format("X: {0} Y: {1} Z: {2}", mRay.X, mRay.Y, mRay.Z), "MouseRay: ");
				ImGui.LabelText(string.Format("{0} (MS: {1})", Math.Round(RenderFrequency,1), Math.Round(RenderPeriod * 1000,2)), "FPS");
				ImGui.Checkbox("Render Scene", ref Engine.Enabled);
				ImGui.ColorPicker4("ClearColor", ref Engine.Video.RendererState.ClearColor);
			}));

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
			GL.ClearColor(Color4.Black);
			Engine.Initialize(Width, Height);

			var rect = ClientRectangle;

			Mouse.SetPosition(Bounds.X + (rect.Width / 2), Bounds.Y + (rect.Height / 2));
			ImGuiController = new ImGuiController(Width, Height);
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
			FrameBuffer.Render(Engine.Render, deltaTime, Engine.Enabled);

			foreach (var item in GUIWIndows.Values)
				item.Draw();
			
			ImGuiController.Render();

			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnResize(EventArgs e)
		{
			if (Width == 0  || Height == 0)
				return;

			GL.Viewport(new Rectangle(0, 0, Width, Height));
			
			FrameBuffer.OnResize(Width, Height);
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
