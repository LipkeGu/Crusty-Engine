using Crusty.Engine.Common;
using Crusty.Engine.Traits;
using OpenTK;
using System.Collections.Generic;

namespace Crusty.Engine.Models
{
	public class Light : Lightning
	{
		public Cube Mesh { get; private set; }

		public Light(Vector3 position, Vector3 lightColor)
		{
			Position = position;
			Mesh = new Cube("Sun", 1, Position, new Vector3(0), new Vector3(1));
			LightColor = lightColor;
		}

		public Light(Vector3 position, Vector3 lightColor, Vector3 attenuation)
		{
			Position = position;
			Mesh = new Cube("Sun", 1, Position, new Vector3(0), new Vector3(1));
			LightColor = lightColor;
			Attenuation = attenuation;
		}

		public void Update(double deltaTime)
		{
			Mesh.Update(null, deltaTime);
		}

		public void Draw(ref GameWorldTime worldTime, ref IList<Light> light, ref Fog fog,
			Matrix4 projMatrix, Matrix4 viewMatrix, bool staticObject = false)
		{
			Mesh.Draw(ref worldTime, ref light, ref fog, projMatrix, viewMatrix, staticObject);
		}
	}
}
