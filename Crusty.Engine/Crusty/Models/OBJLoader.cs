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
					var line = objStream.ReadLine().Replace("  ", " ").Trim();

					#region "Sort out Vertices"
					if (line.StartsWith("v "))
					{
						var vertexParts = line.Split(" ");
						vertices.Add(Functions.CreateVec3(vertexParts[1], vertexParts[2], vertexParts[3]));
						continue;
					}

					if (line.StartsWith("vt "))
					{
						var vertexTexCoord = line.Split(" ");
						texCoords.Add(Functions.CreateVec2(vertexTexCoord[1], vertexTexCoord[2]));
						continue;
					}

					if (line.StartsWith("vn "))
					{
						var vertexNormal = line.Split(" ");
						normals.Add(Functions.CreateVec3(vertexNormal[1], vertexNormal[2], vertexNormal[3]));
						continue;
					}
					#endregion

					#region "Faces"
					if (line.StartsWith("f "))
					{
						var faceParts = line.Split(" ").ToList();
						for (var f = 1; f < faceParts.Count; f++)
						{
							var face = faceParts[f].Split("/");

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


			UploadToVertexArray();
		}
	}
}
