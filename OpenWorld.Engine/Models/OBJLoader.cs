using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenWorld.Engine.Common;

namespace OpenWorld.Engine.Models
{
	public class OBJLoader : Model
	{
		public OBJLoader(string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(name, position, rotation, scale)
		{
			Create(string.Format("Data/Models/{0}.obj", name));
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
			var faces = new List<Vector3>();

			using (var objStream = new StreamReader(filename))
			{
				while (!objStream.EndOfStream)
				{
					var line = objStream.ReadLine().Trim();

					#region "Sort out Vertices"
					if (line.StartsWith("v "))
					{
						var vertexParts = Functions.SplitString(line, " ");
						var vertice = new Vector3(
							float.Parse(vertexParts[1]),
							float.Parse(vertexParts[2]),
							float.Parse(vertexParts[3]));

						vertices.Add(vertice);
						continue;
					}

					if (line.StartsWith("vt "))
					{
						var vertexTexCoord = Functions.SplitString(line, " ");
						var texCoord = new Vector2(
							float.Parse(vertexTexCoord[1]),
							float.Parse(vertexTexCoord[2]));

						texCoords.Add(texCoord);
						continue;
					}

					if (line.StartsWith("vn "))
					{
						var vertexNormal = Functions.SplitString(line, " ");
						var normal = new Vector3(
							float.Parse(vertexNormal[1]),
							float.Parse(vertexNormal[2]),
							float.Parse(vertexNormal[3]));

						normals.Add(normal);
						continue;
					}
					#endregion

					#region "Faces"
					if (line.StartsWith("f "))
					{
						var faceParts = Functions.SplitString(line, " ").ToList();
						for (var f = 0; f < faceParts.Count; f++)
						{
							var face = Functions.SplitString(faceParts[f], "/");
							if (face.Length < 2)
								continue;

							verticeIndice.Add(int.Parse(face[0]));
							texCoordIndice.Add(int.Parse(face[1]));
							normalIndice.Add(int.Parse(face[2]));
						}

					}
					#endregion
				}


				
				objStream.Close();
			}

			int indice = 0;
			foreach (var item in verticeIndice)
			{
				Vertices.Add(vertices[item - 1]);
				Indices.Add(indice++);
			}

			VertexArray.Upload(Vertices, Indices);
		}
	}
}
