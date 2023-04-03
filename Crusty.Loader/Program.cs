using System;
using OpenTK;

namespace Crusty.Loader
{
	class Program
	{
		public static Crusty.Engine.EngineLayer EngineLayer;

		[STAThread]
		static void Main(string[] args)
		{
			var monitor = DisplayDevice.GetDisplay(DisplayIndex.Default);
			EngineLayer = new Engine.EngineLayer(monitor.Width, monitor.Height, OpenTK.Graphics.GraphicsMode.Default, "",
				GameWindowFlags.Fullscreen, monitor, 4, 3, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);

			EngineLayer.Run();
		}
	}
}
