using OpenTK;
using System.Collections.Generic;

namespace Crusty.Engine.Models
{
	public class SkyBox : Quad
	{
		public SkyBox(float size) : base("SkyBox", size, new Vector3(0.0f), new Vector3(0.0f), new Vector3(1.0f))
		{
			var textures = new List<string>
			{
				"Data/Texture/SkyBox/right.jpg",
				"Data/Texture/SkyBox/left.jpg",

				"Data/Texture/SkyBox/top.jpg",
				"Data/Texture/SkyBox/bottom.jpg",

				"Data/Texture/SkyBox/back.jpg",
				"Data/Texture/SkyBox/front.jpg"
			};

			Texture = new Texture(textures);
		}
	}
}
