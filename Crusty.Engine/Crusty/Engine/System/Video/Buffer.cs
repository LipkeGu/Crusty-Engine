using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Crusty.Engine
{
	public class IndexBuffer : Buffer, IDisposable
	{
		public IndexBuffer() : base(BufferTarget.ElementArrayBuffer) { }
	}

	public class VertexBuffer : Buffer, IDisposable
	{
		public VertexBuffer() : base(BufferTarget.ArrayBuffer) { }
	}

	/// <summary>
	/// Allgemeines Buffer Objekt
	/// </summary>
	public class Buffer : IDisposable
	{
		/// <summary>
		/// Die OpenGL-Id des Buffers.
		/// </summary>
		public int Id { get; private set; } = 0;

		/// <summary>
		/// Usage of the Buffer
		/// </summary>
		public BufferUsageHint BufferUsage { get; private set; } = BufferUsageHint.DynamicDraw;

		/// <summary>
		/// Number of Elements in the buffer
		/// </summary>
		public int Elements { get; private set; } = 0;

		/// <summary>
		/// Typ of Buffer (Array, Element, etc...)
		/// </summary>
		public BufferTarget BufferType { get; private set; } = BufferTarget.ArrayBuffer;

		public Buffer(BufferTarget target)
		{
			BufferType = target;
		}

		/// <summary>
		/// Creates the Buffer
		/// </summary>
		public void Create(BufferUsageHint bufferUsage = BufferUsageHint.DynamicDraw, bool bind = true)
		{
			var _id = 0;
			GL.GenBuffers(1, out _id);

			Id = _id;

			BufferUsage = bufferUsage;

			if (bind)
				Bind();
		}

		/// <summary>
		/// Bind the Buffer
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(BufferType, Id);
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<int> data)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Marshal.SizeOf(data[0]), data.ToArray(), BufferUsage);
			Elements = data.Count;
			UnBind();
		}

		public void Update(List<Vector4> data, int offset = 0)
		{
			Bind();
			GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)(data.Count * Vector4.SizeInBytes), data.ToArray());
			Bind();
		}

		public void Update(List<Vector3> data, int offset = 0)
		{
			Bind();
			GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)(data.Count * Vector3.SizeInBytes), data.ToArray());
			Bind();
		}

		public void Update(List<Vector2> data, int offset = 0)
		{
			Bind();
			GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)(data.Count * Vector2.SizeInBytes), data.ToArray());
			Bind();
		}

		public void update(List<float> data, int offset = 0)
		{
			Bind();
			GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)(data.Count * sizeof(float)), data.ToArray());
			Bind();
		}

		/// <summary>
		/// Maps the current bound buffer for manipulation and returns the memory address.
		/// </summary>
		/// <param name="bufferAccess"></param>
		/// <returns>The Memory Address...</returns>
		public IntPtr Map(BufferAccess bufferAccess = BufferAccess.WriteOnly)
		{
			Bind();
			return GL.MapBuffer(BufferType, bufferAccess);
		}

		/// <summary>
		/// Unmap the current mapped buffer.
		/// </summary>
		/// <returns></returns>
		public bool UnMap()
		{
			var retval = GL.UnmapBuffer(BufferType);
			UnBind();

			return retval;
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector3> data)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsage);
			Elements = data.Count;
			UnBind();
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector2> data)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsage);
			Elements = data.Count;
			UnBind();
		}

		/// <summary>
		/// Deaktiviert den Buffer für die Verwendung.
		/// </summary>
		public void UnBind()
		{
			GL.BindBuffer(BufferType, 0);
		}

		public void CleanUp()
		{
			UnBind();
			GL.DeleteBuffer(Id);
		}
		/// <summary>
		/// Deaktiviert den Buffer und gibt die Resourcen wieder frei.
		/// </summary>
		public void Dispose()
		{
			CleanUp();
		}
	}
}
