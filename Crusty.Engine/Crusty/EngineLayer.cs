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

		Matrix4 x;
		private void openGL_GUI_Content()
		{
			ImGui.LabelText(GL.GetString(StringName.Renderer), "Renderer: ");
			ImGui.LabelText(GL.GetString(StringName.Vendor), "Vendor: ");
			ImGui.LabelText(GL.GetString(StringName.Version), "Version: ");

			

		}

		public EngineLayer(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device,
			int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
		{

			x = new Matrix4();

			lastPost = new Vector2(width / 2, height / 2);
			Engine = new CrustyEngine();

			CursorGrabbed = true;

			GUIWIndows.Add("main0", new Window("OpenGL Info", openGL_GUI_Content));
			GUIWIndows.Add("main1", new Window("Crusty Engine", () =>
			{
				var mRay = Engine.Camera.RayPosition;
				ImGui.LabelText(string.Format("X: {0} Y: {1} Z: {2}", mRay.X, mRay.Y, mRay.Z), "MouseRay: ");
			}));

			GUIWIndows.Add("main2", new Window("Camera Viewmatrix", () =>
			{
				var matrix = Engine.Camera.ViewMatrix;

				ImGui.LabelText(matrix.Row0.ToString(), "Row0: ");
				ImGui.LabelText(matrix.Row1.ToString(), "Row1: ");
				ImGui.LabelText(matrix.Row2.ToString(), "Row2: ");
				ImGui.LabelText(matrix.Row3.ToString(), "Row3: ");
			}));
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
			Mouse.SetPosition(lastPost.X, lastPost.Y);
			//Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
			GL.ClearColor(color: Color4.Black);
		
			Engine.Initialize(Width, Height);
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
