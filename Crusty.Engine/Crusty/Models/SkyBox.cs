using OpenTK;
using System.Collections.Generic;

namespace Crusty.Engine.Models
{
	public class SkyBox : Quad
	{
		public SkyBox(float size) : base("SkyBox", size, new Vector3(0.0f), new Vector3(0.0f), new Vector3(1.0f))
		{
			var textures = new List<string>();
			textures.Add("Data/Texture/SkyBox/right.jpg");
			textures.Add("Data/Texture/SkyBox/left.jpg");

			textures.Add("Data/Texture/SkyBox/top.jpg");
			textures.Add("Data/Texture/SkyBox/bottom.jpg");

			textures.Add("Data/Texture/SkyBox/back.jpg");
			textures.Add("Data/Texture/SkyBox/front.jpg");

			Texture = new Texture(textures);
		}
	}
}
