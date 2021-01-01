using OpenTK;
using OpenWorld.Engine.Generators;
using OpenWorld.Engine.Video;

namespace OpenWorld.Engine.Models
{
	public class Terrain : Model
	{
		PerlinNoise perlinNoise = new PerlinNoise();

		public int Width = 64;
		public int Height = 64;

		public Terrain(int width, int height) : base("Terrain", new Vector3(-(width / 2), 0.0f,-(height / 2)), new Vector3(0.0f), new OpenTK.Vector3(width, 1.0f, height))
		{
			Width = width;
			Height = height;
			
			for (var z = 0; z < Height; z++)
				for (var x = 0; x < Width; x++)
				{
					var valX = (float)x / (float)(Width - 1);
					var valZ = (float)z / (float)(Height - 1);

					var _x = (float)valX * Width;
					var _z = (float)valZ * Height;
					var _y = ((float)perlinNoise.Noise(_x * 10, 0, _z * 10) * Width / 2) * MathHelper.Pi;

					Vertices.Add(new Vector3(_x, _y, _z));
				}

			for (var i = 0; i < Height - 1; i++)
				for (var j = 0; j < Width - 1; j++)
				{
					var i0 = j + i * Width;
					var i1 = i0 + 1;
					var i2 = i0 + Width;
					var i3 = i2 + 1;

					Indices.Add(i0);
					Indices.Add(i2);
					Indices.Add(i1);
					Indices.Add(i1);
					Indices.Add(i2);
					Indices.Add(i3);
				}

			VertexArray.Upload(Vertices, Indices);
		}
	}
}
