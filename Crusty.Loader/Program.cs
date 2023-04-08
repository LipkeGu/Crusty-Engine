using System;
using OpenTK;
using OpenTK.Graphics;

namespace Crusty.Loader
{
	class Program
	{
		public static Crusty.Engine.EngineLayer EngineLayer;

		[STAThread]
		static void Main(string[] args)
		{
			var contextFlags = GraphicsContextFlags.ForwardCompatible;
			var width = 1280;
			var height = 720;
			var gamewindowFlags = GameWindowFlags.Default;

			var monitor = DisplayDevice.GetDisplay(DisplayIndex.Default);
			width = monitor.Width;
			height = monitor.Height;
			gamewindowFlags = GameWindowFlags.Default;

			EngineLayer = new Engine.EngineLayer(width, height, GraphicsMode.Default, "", gamewindowFlags, monitor, 4, 3, contextFlags);
			EngineLayer.Run();
		}
	}
}
