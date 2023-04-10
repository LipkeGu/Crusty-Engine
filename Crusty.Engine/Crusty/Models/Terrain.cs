using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using Crusty.Engine.Crusty.Models.Interface;

namespace Crusty.Engine.Models
{
	public class Terrain : Model, ITerrain
	{
		public int Width { get; set; } = 0;
		public int Height { get; set; } = 0;

		public Dictionary<int, Dictionary<int, float>> Heights;

		public Terrain(string filename, string normmap) : base("Terrain",
			new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f), new OpenTK.Vector3(1f, 1f, 1f))
		{
			Heights = new Dictionary<int, Dictionary<int, float>>();

			using (var normalmap = new Bitmap(normmap))
			{
				using (var heightmap = new Bitmap(filename))
				{
					Width = heightmap.Width;
					Height = heightmap.Height;

					for (var z = 0; z < Height; z++)
					{
						Heights.Add(z, new Dictionary<int, float>());

						for (var x = 0; x < Width; x++)
						{
							var pixel = normalmap.GetPixel(x, z);

							Normals.Add(new Vector3(pixel.R, pixel.G, pixel.B));
							Vertices.Add(new Vector3(x, Heights[z][x] = heightmap.GetPixel(x, z).B, z));
						}
					}
				}
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

			UploadToVertexArray();
		}

		public float QueryHeightAt(int x, int z)
		{
			var terrainZ = z;
			var terrainX = x;

			var gridSquareSize = Width / (Heights.Count - 1);

			var tileX = terrainX / gridSquareSize;
			var tileZ = terrainZ / gridSquareSize;

			float xCoord = (terrainX % gridSquareSize) / gridSquareSize;
			float zCoord = (terrainZ % gridSquareSize) / gridSquareSize;
			
			var newHeight = 0.0f;

			if (tileX < 0 || tileX + 1 >= Heights.Count || tileZ < 0 || tileZ + 1 >= Heights.Count)
				return newHeight;

			if (xCoord <= (1 - zCoord))
			{
				newHeight = Vector3.BaryCentric(
					new Vector3(0, Heights[tileZ][tileX], 0),
					new Vector3(1, Heights[tileZ][tileX + 1], 0),
					new Vector3(0, Heights[tileZ + 1][tileX], 1),
					xCoord, zCoord).Y;
			}
			else
			{
				newHeight = Vector3.BaryCentric(new Vector3(1, Heights[tileZ][tileX + 1], 0),
					new Vector3(1, Heights[tileZ + 1][tileX + 1], 1),
					new Vector3(0, Heights[tileZ + 1][tileX], 1),
					xCoord, zCoord).Y;
			}

			return newHeight;
		}
	}
}
