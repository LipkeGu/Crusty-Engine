using OpenWorld.Engine.Video;
using System;
using OpenTK;
using System.Collections.Generic;
using OpenWorld.Engine.Traits;
using OpenTK.Graphics.OpenGL4;
using OpenWorld.Engine.Common;

namespace OpenWorld.Engine.Models
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

	public class Model : MoveAble, IDisposable
	{
		public VertexArray VertexArray { get; private set; }
		public Shader Shader;
		public Texture texture;

		public string Name;

		public List<int> Indices = new List<int>();
		public List<Vector3> Vertices = new List<Vector3>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<Vector2> TexCoords = new List<Vector2>();

		protected Model(string name, Vector3 position, Vector3 rotation, Vector3 scale) {
			Name = name;
			Scale = scale;
			Rotation = rotation;
			Position = position;
			Shader = new Shader();
			texture = new Texture(string.Format("Data/Texture/{0}.png", Name));
			Shader.Create(string.Format("Data/Shaders/{0}.glsl", Name));
			VertexArray = new VertexArray();
			VertexArray.Create();
		}

		

		public virtual void Update(double deltatime)
		{
		}

		public virtual void Draw(ref GameWorldTime worldTime, ref Camera camera, bool staticObject = false)
		{
			texture.Bind();
			var modelMatrix = Functions.CreateTransformationMatrix(Position, Rotation, Scale);
			VertexArray.Draw(ref worldTime, ref Shader, ref camera, ref modelMatrix, Scale, staticObject);
			texture.UnBind();
		}

		public void CleanUp()
		{
			texture.CleanUp();
			Shader.Cleanup();
			VertexArray.CleanUp();
			Vertices.Clear();
			Indices.Clear();
		}

		public void Dispose()
		{
			texture.Dispose();
			Shader.Dispose();
			VertexArray.Dispose();
		}
	}
}
