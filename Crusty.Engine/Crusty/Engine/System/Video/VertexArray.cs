using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Crusty.Engine.Common;
using Crusty.Engine.Models;

namespace Crusty.Engine
{
	public class VertexArray : IDisposable
	{
		private int attributeCount = 0;
		public bool Instanced = false;

		public List<Buffer> VertexBuffers;
		public IndexBuffer IndexBuffer;

		public int VerticeCount { get; private set; } = 0;

		public int Id { get; private set; } = 0;

		public VertexArray()
		{
			VertexBuffers = new List<Buffer>();
		}

		public void Create()
		{
			var _id = 0;
			GL.GenVertexArrays(1, out _id);
			Id = _id;

			Bind();
		}

		public void Bind()
		{
			GL.BindVertexArray(Id);

			foreach (var vbo in VertexBuffers)
				vbo.Bind();

			for (var i = 0; i < attributeCount; i++)
				GL.EnableVertexAttribArray(i);

			if (IndexBuffer != null)
				IndexBuffer.Bind();
		}

		public void Upload(List<Vector2> data)
		{
			var vbo = new VertexBuffer();
			vbo.Create();
			vbo.Upload(data);

			if (VertexBuffers.Count == 0)
				VerticeCount = vbo.Elements;

			VertexBuffers.Add(vbo);

			Bind();

			var elements = Marshal.SizeOf(data[0]) / sizeof(float);
			GL.VertexAttribPointer(attributeCount, elements, VertexAttribPointerType.Float, false, elements * sizeof(float), 0);
			GL.EnableVertexAttribArray(attributeCount);

			if (Instanced)
				GL.VertexAttribDivisor(attributeCount, 1);

			attributeCount++;

			UnBind();
		}

		public void Upload(List<Vector3> data, List<int> indices, bool instanced = false)
		{
			var vbo = new VertexBuffer();
			vbo.Create();
			vbo.Upload(data);

			if (VertexBuffers.Count == 0)
				VerticeCount = vbo.Elements;

			VertexBuffers.Add(vbo);

			if (indices.Count != 0)
			{
				IndexBuffer = new IndexBuffer();
				IndexBuffer.Create();

				IndexBuffer.Upload(indices);
			}

			Bind();

			var elements = Marshal.SizeOf(data[0]) / sizeof(float);
			GL.VertexAttribPointer(attributeCount, elements, VertexAttribPointerType.Float, false, elements * sizeof(float), 0);
			GL.EnableVertexAttribArray(attributeCount);
			
			if (Instanced)
				GL.VertexAttribDivisor(attributeCount, 1);

			attributeCount++;

			UnBind();
		}

		public void UnBind()
		{
			foreach (var vbo in VertexBuffers)
				vbo.UnBind();

			if (IndexBuffer != null)
				IndexBuffer.UnBind();

			for (var i = 0; i < attributeCount; i++)
				GL.DisableVertexAttribArray(i);

			GL.BindVertexArray(0);
		}

		public void CleanUp()
		{
			UnBind();

			foreach (var vertexBuffer in VertexBuffers)
				vertexBuffer.CleanUp();

			if (IndexBuffer != null)
				IndexBuffer.CleanUp();

			VertexBuffers.Clear();
			GL.DeleteVertexArray(Id);
		}

		public void Draw(IShader shader = null)
		{
			var indicesCount = 0;

			if (IndexBuffer != null)
				indicesCount = IndexBuffer.Elements;

			if (VerticeCount == 0)
				return;

			if (shader != null)
				shader.Use();

			Bind();
			
			if (indicesCount == 0)
				GL.DrawArrays(PrimitiveType.Triangles, 0, VerticeCount);
			else
				GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);

			UnBind();

			if (shader != null)
				shader.Unuse();
		}

		private void Draw(ref GameWorldTime worldTime, ref IShader shader, ref IList<Light> light, ref Fog fog,
			Matrix4 viewMatrix, Matrix4 projectionMatrix, Matrix4 transform, Vector3 scale)
		{
			
			shader.Use();
			shader.Set_Vec3("Scale", scale);

			shader.Set_Float("gradient", fog.Gradient);
			shader.Set_Float("density", fog.Density);
			shader.Set_Light(light);
			shader.Set_Vec3("fogColor", fog.Color);
			shader.Set_Float("AmbientStrength", worldTime.AmbientStrength);
			shader.Set_Mat4("projMatrix", projectionMatrix);
			shader.Set_Mat4("viewMatrix", viewMatrix);
			shader.Set_Mat4("modelMatrix", transform);

			Draw();

			shader.Unuse();
		}

		public void Draw(ref GameWorldTime worldTime, ref IShader shader, ref IList<Light> light, ref Fog fog,
			Matrix4 projMatrix, Matrix4 viewMatrix, Matrix4 transform, Vector3 scale, bool fixedModel = false)
		{
			var view = viewMatrix;

			if (fixedModel)
				view.Row3 = new Vector4(0, 0, 0, view.Row3.W);

			Draw(ref worldTime, ref shader, ref light, ref fog, view, projMatrix, transform, scale);
		}

		public void Dispose()
		{
			if (IndexBuffer != null)
				IndexBuffer.Dispose();

			foreach (var vertexBuffer in VertexBuffers)
				vertexBuffer.Dispose();
		}
	}
}
