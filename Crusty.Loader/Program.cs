using System;
using OpenTK;
using OpenTK.Graphics;

namespace Crusty.Loader
{
	class Program
	{
		public static Engine.EngineLayer EngineLayer;

		[STAThread]
		static void Main(string[] args)
		{
			var width = 1280;
			var height = 720;
			var gamewindowFlags = GameWindowFlags.Default;
			var monitor = DisplayDevice.GetDisplay(DisplayIndex.Default);

			var contextFlags = GraphicsContextFlags.ForwardCompatible;
#if DEBUG
			contextFlags |= GraphicsContextFlags.Debug;
#else
			width = monitor.Width;
			height = monitor.Height;
			gamewindowFlags = GameWindowFlags.Fullscreen;
#endif
			EngineLayer = new Engine.EngineLayer(width, height, GraphicsMode.Default, "", gamewindowFlags, monitor, 4, 3, contextFlags);
			EngineLayer.Run();
		}
	}
}
