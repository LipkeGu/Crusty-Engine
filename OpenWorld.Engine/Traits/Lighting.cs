using System;
using OpenTK;

namespace OpenWorld.Engine.Traits
{
	public class Lighting
	{
		public Vector3 Position { get; internal set; }
		public Vector3 LightColor { get; internal set; }
		public float Attenuation { get; internal set; }

	}
}
