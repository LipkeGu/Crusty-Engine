using Crusty.Engine.Common;
using Crusty.Engine.Crusty.Models.Interface;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Crusty.Engine.Models
{
	public class Terrain : Model, ITerrain
	{
		public int Width { get; set; } = 0;
		public int Height { get; set; } = 0;

		public string NormalMap { get; set; }

		public string HeightMap { get; set; }



		public Dictionary<int, Dictionary<int, float>> Heights
			= new Dictionary<int, Dictionary<int, float>>();

		public Terrain(string heightMap, string normmap) : base("Terrain",
			new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f), new Vector3(1f, 1f, 1f))
		{
			NormalMap = normmap;
			HeightMap = heightMap;
			Width = 1024;
			Height = 1024;

			using (var normalmap = new Bitmap(NormalMap))
			{
				using (var heightmap = new Bitmap(HeightMap))
				{
					Width = heightmap.Width;
					Height = heightmap.Height;

					for (var z = 0; z < Height; z++)
					{
						if (!Heights.ContainsKey(z))
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

		public void Import()
		{
			var xml = new XmlDocument();
			xml.Load("Data/Terrains/Export.xml");
			var tiles = xml.GetElementsByTagName("Tile");

			foreach (XmlNode tile in tiles)
			{
				var vec = Functions.CreateVec3(tile.Attributes["x"].Value,
					tile.Attributes["y"].Value, tile.Attributes["z"].Value);

				Vertices.Add(vec);

				if (!Heights.ContainsKey((int)vec.Z))
					Heights.Add((int)vec.Z, new Dictionary<int, float>());

				Heights[(int)vec.Z][(int)vec.X] = vec.Y;
			}
		}

		public void Export(string name)
		{
			var terrain = new XElement("Terrain");

			foreach (var vertice in Vertices)
				terrain.Add(new XElement("Tile", new XAttribute("x", vertice.X), new XAttribute("y", vertice.Y), new XAttribute("z", vertice.Z)));

			Directory.CreateDirectory("Data/Terrains");
			XElement terraisn = new XElement("Engine", new XElement("Terrains", terrain));
			terraisn.Save("Data/Terrains/Export.xml", SaveOptions.OmitDuplicateNamespaces);
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
