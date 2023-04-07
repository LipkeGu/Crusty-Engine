using System;
using OpenTK;

namespace Crusty.Engine.Traits
{
	public class Lighting
	{
		public Vector3 Position { get; internal set; }
		public Vector3 LightColor { get; internal set; }
		public Vector3 Attenuation { get; internal set; } = new Vector3(1, 0, 0);
	}
}
