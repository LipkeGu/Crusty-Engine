using System;
using OpenTK;
using Crusty.Engine.Traits;

namespace Crusty.Engine.Models
{
	public class Light : Lighting
	{
		public Light(Vector3 position, Vector3 lightColor, float attenuation)
		{
			Position = position;
			LightColor = lightColor;
			Attenuation = attenuation;
		}
	}
}
