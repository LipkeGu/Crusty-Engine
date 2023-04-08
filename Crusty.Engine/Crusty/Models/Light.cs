using System;
using OpenTK;
using Crusty.Engine.Traits;
using Crusty.Engine.Common;
using System.Collections.Generic;

namespace Crusty.Engine.Models
{
	public class Light : Lighting
	{
		public Quad Sun { get; private set; }

		public Light(Vector3 position, Vector3 lightColor)
		{
			Position = position;
			Sun = new Quad("Sun", 1, Position, new Vector3(0), new Vector3(1));
			LightColor = lightColor;
		}

		public Light(Vector3 position, Vector3 lightColor, Vector3 attenuation)
		{
			Position = position;
			Sun = new Quad("Sun", 1, Position, new Vector3(0), new Vector3(1));
			LightColor = lightColor;
			Attenuation = attenuation;
		}

		public void Update(double deltaTime)
		{
			Sun.Update(null, deltaTime);
		}

		public void Draw(ref GameWorldTime worldTime, ref IList<Light> light, ref Fog fog,
			Matrix4 projMatrix, Matrix4 viewMatrix, bool staticObject = false)
		{
			Sun.Draw(ref worldTime, ref light, ref fog, projMatrix, viewMatrix, staticObject);
		}
	}
}
