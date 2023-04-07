using System;
using OpenTK;
using Crusty.Engine.Traits;

namespace Crusty.Engine.Models
{
	public class Light : Lighting
	{
		public Light(Vector3 position, Vector3 lightColor)
		{
			Position = position;
			LightColor = lightColor;
		}

		public Light(Vector3 position, Vector3 lightColor, Vector3 attenuation)
		{
			Position = position;
			LightColor = lightColor;
			Attenuation = attenuation;
		}
	}
}
