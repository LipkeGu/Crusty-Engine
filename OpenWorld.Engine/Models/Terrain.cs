﻿using OpenTK;
using OpenWorld.Engine.Common;
using OpenWorld.Engine.Generators;
using OpenWorld.Engine.Video;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenWorld.Engine.Models
{
	public class Terrain : Model
	{
		PerlinNoise perlinNoise = new PerlinNoise();

		public int Width = 0;
		public int Height = 0;
		public int SiZE = 10;

		public Dictionary<int, Dictionary<int, float>> Heights = new Dictionary<int, Dictionary<int, float>>();


		public Terrain(string filename) : base("Terrain", new Vector3(0, 0, 0), new Vector3(0.0f), new OpenTK.Vector3(1, 1f, 1))
		{
			Heights = new Dictionary<int, Dictionary<int, float>>();

			using (var heightmap = new Bitmap(filename))
			{
				Width = heightmap.Width;
				Height = heightmap.Height;

				for (var z = 0; z < Height; z++)
				{
					Heights.Add(z, new Dictionary<int, float>());
					for (var x = 0; x < Width; x++)
					{
						var pixel = heightmap.GetPixel(x, z).B;

						Vertices.Add(new Vector3(x, Heights[z][x] = pixel, z));
					}
				}
			}

			for (var i = 0; i < Height - 1; i++)
				for (var j = 0; j < Width - 1; j++)
				{
					var i0 = j + i * (int)Width;
					var i1 = i0 + 1;
					var i2 = i0 + (int)Width;
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

		public float GetHeightAt(int x, int z)
		{
			var newPos = 0.0f;
			var _z = z;
			var _x = x;

			int terrainX = _x;
			int terrainZ = _z;

			int gridSquareSize = Width / (Heights.Count - 1);
			int gridX = terrainX / gridSquareSize;
			int gridZ = terrainZ / gridSquareSize;

			float xCoord = (terrainX % gridSquareSize) / gridSquareSize;
			float zCoord = (terrainZ % gridSquareSize) / gridSquareSize;

			if (gridX < 0 || gridX + 1 >= (int)Heights.Count || gridZ < 0 || gridZ + 1 >= (int)Heights.Count)
				return 0.0f;

				if (xCoord <= (1 - zCoord))
			{
				newPos = Functions.BarryCentric(
					new Vector3(0, Heights[gridZ][gridX], 0),
					new Vector3(1, Heights[gridZ][gridX + 1], 0),
					new Vector3(0, Heights[gridZ + 1][gridX], 1),
					new Vector2(xCoord, zCoord));
			}
			else
			{
				newPos = Functions.BarryCentric(
					new Vector3(1, Heights[gridZ][gridX + 1], 0),
					new Vector3(1, Heights[gridZ + 1][gridX + 1], 1),
					new Vector3(0, Heights[gridZ + 1][gridX], 1),
					new Vector2(xCoord, zCoord));
			}

			return newPos;
		}
	}
}
