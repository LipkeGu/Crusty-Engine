using System;
using OpenTK;
using System.Collections.Generic;
using Crusty.Engine.Traits;
using OpenTK.Graphics.OpenGL4;
using Crusty.Engine.Common;
using Crusty.Engine.Common.Traits;
using Crusty.Engine.System;

namespace Crusty.Engine.Models
{
	public class Triangle : Model
	{
		public Triangle(string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(name, position, rotation, scale)
		{
			Vertices.Add(new Vector3(-0.5f, -0.5f, 0.0f));
			Vertices.Add(new Vector3(0.5f, -0.5f, 0.0f));
			Vertices.Add(new Vector3(0.0f, 0.5f, 0.0f));

			UploadToVertexArray();
		}
	}

	public class Quad : Model
	{
		public float Size { get; private set; }

		public Quad(string name, float size, Vector3 position, Vector3 rotation, Vector3 scale) : base(name, position, rotation, scale)
		{
			Size = size;

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

			UploadToVertexArray();
		}
	}

	public class Model : MoveAble, IDisposable
	{
		public VertexArray VertexArray { get; private set; }
		public IShader Shader;
		public Texture Texture;
		public string Name;

		public float shineDamper = 1.0f;
		public float reflectivity = 0.0f;

		public List<int> Indices = new List<int>();
		public List<Vector3> Vertices = new List<Vector3>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<Vector2> TexCoords = new List<Vector2>();

		protected Model(string name, Vector3 position, Vector3 rotation, Vector3 scale)
		{
			Name = name;
			Scale = scale;
			Rotation = rotation;
			Position = position;
			Shader = new Shader();
			Texture = new Texture(string.Format("Data/Texture/{0}.png", Name));
			Shader.Create(string.Format("Data/Shaders/{0}.glsl", Name));

			VertexArray = new VertexArray();
			VertexArray.Create();
		}

		protected Model(string name)
		{
			Name = name;
			Shader = new Shader();
			Texture = new Texture(string.Format("Data/Texture/{0}.png", Name));
			Shader.Create(string.Format("Data/Shaders/{0}.glsl", Name));

			VertexArray = new VertexArray();
			VertexArray.Create();
		}

		public void Set_Shine_Variables(float shineDamper, float reflectivity)
		{
			Shader.Use();
			Shader.Set_Float("shineDamper", shineDamper);
			Shader.Set_Float("reflectivity", reflectivity);
			Shader.Unuse();
		}

		public virtual void Update(Terrain terrain, double deltatime, bool terrainDebug = false)
		{
			if (terrain != null)
				Position.Y = terrain.QueryHeightAt((int)Position.X, (int)Position.Z);
			
			UpdateModelMatrix();

			Set_Shine_Variables(shineDamper, reflectivity);
		}

		public virtual void Draw(ref GameWorldTime worldTime, ref IList<Light> light, ref Fog fog,
			Matrix4 projMatrix, Matrix4 viewMatrix, bool staticObject = false)
		{
			Texture.Bind();
			VertexArray.Draw(ref worldTime, ref Shader, ref light, ref fog, projMatrix, viewMatrix, ModelMatrix, Scale, staticObject);
			Texture.UnBind();
		}


		public void UploadToVertexArray()
		{
			if (Vertices.Count != 0)
			{
				VertexArray.Upload(Vertices, Indices);
				Vertices.Clear();
			}

			if (TexCoords.Count != 0)
			{
				VertexArray.Upload(TexCoords);
				TexCoords.Clear();
			}

			if (Normals.Count != 0)
			{
				VertexArray.Upload(Normals, Indices);
				Normals.Clear();
			}
		}

		public void CleanUp()
		{
			Texture.CleanUp();
			Shader.Cleanup();
			VertexArray.CleanUp();
			Vertices.Clear();
			Normals.Clear();
			Indices.Clear();
		}

		public void Dispose()
		{
			CleanUp();

			Texture.Dispose();
			Shader.Dispose();
			VertexArray.Dispose();
		}
	}
}
