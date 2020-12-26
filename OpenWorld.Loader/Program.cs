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
			EngineLayer = new Engine.EngineLayer(1280, 768, OpenTK.Graphics.GraphicsMode.Default, "OpenWorld",
				GameWindowFlags.Default, DisplayDevice.Default, 3, 3, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);
			EngineLayer.Run();
		}
	}
}
