using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crusty.Engine.Common;
using System.Globalization;

namespace Crusty.Engine.Models
{
	public class OBJLoader : Model
	{
		public OBJLoader(string name, Vector3 position, Vector3 rotation, Vector3 scale)
			: base(name, position, rotation, scale)
		{
			Create(string.Format("Data/Models/{0}.mdl", name));
		}

		public OBJLoader(string name) : base(name)
		{
			Create(string.Format("Data/Models/{0}.mdl", name));
		}

		public void Create(string filename)
		{
			if (!File.Exists(filename))
			{
				Console.WriteLine("[E] Could not find: {0}", filename);
				return;
			}

			var vertices = new List<Vector3>();
			var verticeIndice = new List<int>();
			var texCoords = new List<Vector2>();
			var texCoordIndice = new List<int>();
			var normals = new List<Vector3>();
			var normalIndice = new List<int>();

			using (var objStream = new StreamReader(filename))
			{
				while (!objStream.EndOfStream)
				{
					var line = objStream.ReadLine().Trim();

					#region "Sort out Vertices"
					if (line.StartsWith("v "))
					{
						var vertexParts = Functions.SplitString(line.Replace("  ", " "), " ");

						var vertice = new Vector3(
							float.Parse(vertexParts[1], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(vertexParts[2], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(vertexParts[3], CultureInfo.InvariantCulture.NumberFormat)
							);

						vertices.Add(vertice);
						continue;
					}

					if (line.StartsWith("vt "))
					{
						var vertexTexCoord = Functions.SplitString(line, " ");
						var texCoord = new Vector2(
							float.Parse(vertexTexCoord[1], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(vertexTexCoord[2], CultureInfo.InvariantCulture.NumberFormat)
							);

						texCoords.Add(texCoord);
						continue;
					}

					if (line.StartsWith("vn "))
					{
						var vertexNormal = Functions.SplitString(line, " ");
						var normal = new Vector3(
							float.Parse(vertexNormal[1], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(vertexNormal[2], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(vertexNormal[3], CultureInfo.InvariantCulture.NumberFormat)
							);

						normals.Add(normal);
						continue;
					}
					#endregion

					#region "Faces"
					if (line.StartsWith("f "))
					{
						var faceParts = Functions.SplitString(line, " ").ToList();
						for (var f = 1; f < faceParts.Count; f++)
						{
							var face = Functions.SplitString(faceParts[f], "/");

							verticeIndice.Add(int.Parse(face[0]) - 1);
							if (face.Length > 1)
							{
								texCoordIndice.Add(int.Parse(face[1]) - 1);
								normalIndice.Add(int.Parse(face[2]) - 1);
							}
						}

					}
					#endregion
				}

				objStream.Close();
			}

			int indice = 0;

			foreach (var item in verticeIndice)
			{
				Vertices.Add(vertices[item]);
				Indices.Add(indice++);
			}

			foreach (var item in texCoordIndice)
				TexCoords.Add(texCoords[item]);

			foreach (var item in normalIndice)
				Normals.Add(normals[item]);

			VertexArray.Upload(Vertices, Indices);
			if (TexCoords.Count != 0)
				VertexArray.Upload(TexCoords);

			if (Normals.Count != 0)
				VertexArray.Upload(Normals, new List<int>());
		}
	}
}
