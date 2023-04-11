using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Crusty.Engine
{
	public class IndexBuffer : Buffer, IDisposable
	{
		public IndexBuffer(int capacity) : base(BufferTarget.ElementArrayBuffer, capacity) { }
	}

	public class VertexBuffer : Buffer, IDisposable
	{
		public VertexBuffer(int capacity) : base(BufferTarget.ArrayBuffer, capacity) { }
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

		public int Size { get; private set; } = 0;

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

		public Buffer(BufferTarget target, int capacity = 16384)
		{
			BufferType = target;
			var _id = 0;
			GL.GenBuffers(1, out _id);

			Id = _id;
			Size = capacity;

			Bind();
			GL.BufferData(BufferType, Size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
			UnBind();
		}

		/// <summary>
		/// Bind the Buffer
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(BufferType, Id);
		}

		public void Update(List<int> data, int offset = 0)
		{
			var _size = (data.Count * sizeof(int));

			Bind();
			if (Size < _size)
			{
				GL.BufferData(BufferType, _size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector4> data, int offset = 0)
		{
			var _size = (data.Count * Vector4.SizeInBytes);

			Bind();
			if (Size < _size)
			{
				GL.BufferData(BufferType, _size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector3> data, int offset = 0)
		{
			var _size = (data.Count * Vector3.SizeInBytes);

			Bind();
			if (Size < _size)
			{
				GL.BufferData(BufferType, _size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector2> data, int offset = 0)
		{
			var _size = (data.Count * Vector2.SizeInBytes);

			Bind();
			if (Size < _size)
			{
				GL.BufferData(BufferType, _size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<float> data, int offset = 0)
		{
			Bind();
			GL.BufferSubData(BufferType, (IntPtr)offset, (IntPtr)(data.Count * sizeof(float)), data.ToArray());
			UnBind();
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
		public void Upload(List<int> data)
		{
			Elements = data.Count;
			Update(data);
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector3> data)
		{
			Elements = data.Count;
			Update(data);
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector2> data)
		{
			Elements = data.Count;
			Update(data);
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
