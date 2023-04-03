using System;
using OpenTK;

namespace Crusty.Engine.Models
{
	public class Fog
	{
		public float Density { get; private set; } = 0.0035f;
		public float Gradient { get; private set; } = 5.0f;
		public Vector3 Color { get; private set; } = new Vector3(38, 50, 56) / 255;

		public Fog()
		{
		}

		public Fog(Vector3 color, float density, float gradient)
		{
			Color = color;
			Density = density;
			Gradient = gradient;
		}

		public void Update()
		{

		}
	}
}
