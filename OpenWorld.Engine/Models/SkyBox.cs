using OpenTK;
using OpenWorld.Engine.Common;
using System.Collections.Generic;

namespace OpenWorld.Engine.Models
{
	public class SkyBox : Model
	{
		public SkyBox(int size) : base ("SkyBox", new Vector3(0.0f), new Vector3(0.0f), new Vector3(1.0f))
		{
			var textures = new List<string>();
			textures.Add("Data/Texture/SkyBox/right.jpg");
			textures.Add("Data/Texture/SkyBox/left.jpg");

			textures.Add("Data/Texture/SkyBox/top.jpg");
			textures.Add("Data/Texture/SkyBox/bottom.jpg");

			textures.Add("Data/Texture/SkyBox/back.jpg");
			textures.Add("Data/Texture/SkyBox/front.jpg");

			Vertices.Add(new Vector3(-size, -size, -size));
			Vertices.Add(new Vector3(size, -size, -size));
			Vertices.Add(new Vector3(size, size, -size));
			Vertices.Add(new Vector3(size, size, -size));
			Vertices.Add(new Vector3(-size, size, -size));
			Vertices.Add(new Vector3(-size, -size, -size));

			Vertices.Add(new Vector3(-size, -size, size));
			Vertices.Add(new Vector3(size, -size, size));
			Vertices.Add(new Vector3(size, size, size));
			Vertices.Add(new Vector3(size, size, size));
			Vertices.Add(new Vector3(-size, size, size));
			Vertices.Add(new Vector3(-size, -size, size));

			Vertices.Add(new Vector3(-size, size, size));
			Vertices.Add(new Vector3(-size, size, -size));
			Vertices.Add(new Vector3(-size, -size, -size));
			Vertices.Add(new Vector3(-size, -size, -size));
			Vertices.Add(new Vector3(-size, -size, size));
			Vertices.Add(new Vector3(-size, size, size));

			Vertices.Add(new Vector3(size, size, size));
			Vertices.Add(new Vector3(size, size, -size));
			Vertices.Add(new Vector3(size, -size, -size));
			Vertices.Add(new Vector3(size, -size, -size));
			Vertices.Add(new Vector3(size, -size, size));
			Vertices.Add(new Vector3(size, size, size));

			Vertices.Add(new Vector3(-size, -size, -size));
			Vertices.Add(new Vector3(size, -size, -size));
			Vertices.Add(new Vector3(size, -size, size));
			Vertices.Add(new Vector3(size, -size, size));
			Vertices.Add(new Vector3(-size, -size, size));
			Vertices.Add(new Vector3(-size, -size, -size));

			Vertices.Add(new Vector3(-size, size, -size));
			Vertices.Add(new Vector3(size, size, -size));
			Vertices.Add(new Vector3(size, size, size));
			Vertices.Add(new Vector3(size, size, size));
			Vertices.Add(new Vector3(-size, size, size));
			Vertices.Add(new Vector3(-size, size, -size));

			VertexArray.Upload(Vertices, Indices);
			texture = new Video.Texture(textures);
		}
	}


}
