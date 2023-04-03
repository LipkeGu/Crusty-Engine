using Crusty.Engine.Video;
using System;
using OpenTK;
using System.Collections.Generic;
using Crusty.Engine.Traits;
using OpenTK.Graphics.OpenGL4;
using Crusty.Engine.Common;

namespace Crusty.Engine.Models
{
	public class Triangle : Model
	{
		public Triangle(string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(name, position, rotation, scale)
		{
			Vertices.Add(new Vector3(-0.5f, -0.5f, 0.0f));
			Vertices.Add(new Vector3(0.5f, -0.5f, 0.0f));
			Vertices.Add(new Vector3(0.0f, 0.5f, 0.0f));

			VertexArray.Upload(Vertices, Indices);
		}
	}

	public class Model : IDisposable
	{
		public VertexArray VertexArray { get; private set; }
		public Shader Shader;
		public Texture Texture;
		public Matrix4 ModelMatrix;
		public string Name;

		public Vector3 Position = new Vector3(0.0f, 6.0f, 0.0f);
		public Vector3 Rotation = new Vector3(0.0f, 0.0f, 0.0f);
		public Vector3 Scale = new Vector3(1.0f, 1.0f, 1.0f);

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


		public virtual void Update(double deltatime, bool terrainDebug = false)
		{
			ModelMatrix = Functions.CreateTransformationMatrix(Position, Rotation, Scale);
			Shader.Use();
			Shader.Set_Int("EnableTestColor", terrainDebug ? 1 : 0);
			Shader.Unuse();
		}

		public virtual void Draw(ref GameWorldTime worldTime, ref Fog fog, ref Camera camera, bool staticObject = false)
		{
			Texture.Bind();
			VertexArray.Draw(ref worldTime, ref Shader, ref fog, ref camera, ref ModelMatrix, Scale, staticObject);
			Texture.UnBind();
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
			Texture.Dispose();
			Shader.Dispose();
			VertexArray.Dispose();
		}
	}
}
