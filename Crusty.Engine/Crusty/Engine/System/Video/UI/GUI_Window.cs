using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.UI
{
	public class Window : IControl
	{
		Dictionary<string, Control> Controls;

		public string Text { get; set; }

		public string Name { get; set; }

		public ImGuiWindowFlags Flags { get; set; } = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.AlwaysUseWindowPadding;

		public Vector2 Position { get; set; }

		Action Action { get; set; }

		public Window(string text, Action action)
		{
			Action = action;
			Controls = new Dictionary<string, Control>();
			Text = text;
			Action = action;
		}

		bool open = true;
		public void Draw()
		{
			

			ImGui.Begin(Text, ref open, Flags);
			Action();
			ImGui.End();
		}
	}
}
