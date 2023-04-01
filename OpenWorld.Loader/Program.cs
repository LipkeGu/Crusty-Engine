using System;
using OpenTK;

namespace OpenWorld.Loader
{
	class Program
	{
		public static OpenWorld.Engine.EngineLayer EngineLayer;

		[STAThread]
		static void Main(string[] args)
		{
			var monitor = DisplayDevice.GetDisplay(DisplayIndex.Default);
			EngineLayer = new Engine.EngineLayer(monitor.Width, monitor.Height, OpenTK.Graphics.GraphicsMode.Default, "OpenWorld",
				GameWindowFlags.Fullscreen, monitor, 4, 3, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);

			EngineLayer.Run();
		}
	}
}
