using OpenTK;
using System.Collections.Generic;

namespace OpenWorld.Engine.Models
{
	public class SkyBox : Model
	{
		public float Size { get; private set; }

		public SkyBox(float size) : base("SkyBox", new Vector3(0.0f), new Vector3(0.0f), new Vector3(1.0f))
		{
			Size = size;

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
			Texture = new Video.Texture(textures);
			Vertices.Clear();
		}
	}
}
