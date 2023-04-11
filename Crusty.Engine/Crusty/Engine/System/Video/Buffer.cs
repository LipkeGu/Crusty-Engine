using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

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

	public class Buffer : IDisposable
	{
		public int Id { get; private set; } = 0;

		public int Size { get; private set; } = 0;

		public BufferUsageHint BufferUsage { get; private set; } = BufferUsageHint.DynamicDraw;

		public int Elements { get; private set; } = 0;

		public BufferTarget BufferType { get; private set; } = BufferTarget.ArrayBuffer;

		public Buffer(BufferTarget target, int capacity = 16384)
		{
			BufferType = target;
			var _id = 0;
			GL.GenBuffers(1, out _id);

			Id = _id;
			Size = capacity;

			Bind();
			GL.BufferData(BufferType, Size, IntPtr.Zero, BufferUsage);
			UnBind();
		}

		public void Bind()
		{
			GL.BindBuffer(BufferType, Id);
		}

		public void Update(List<int> data)
		{
			var _size = (data.Count * sizeof(int));

			Bind();
			if (Size != _size)
			{
				GL.BufferData(BufferType, _size, data.ToArray(), BufferUsage);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector4> data)
		{
			var _size = (data.Count * Vector4.SizeInBytes);

			Bind();
			if (Size != _size)
			{
				GL.BufferData(BufferType, _size, data.ToArray(), BufferUsage);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector3> data)
		{
			var _size = (data.Count * Vector3.SizeInBytes);

			Bind();
			if (Size != _size)
			{
				GL.BufferData(BufferType, _size, data.ToArray(), BufferUsage);
				Size = _size;
			}
			else
				GL.BufferSubData(BufferType, IntPtr.Zero, (IntPtr)(_size), data.ToArray());

			UnBind();
		}

		public void Update(List<Vector2> data)
		{
			var _size = (data.Count * Vector2.SizeInBytes);

			Bind();
			if (Size != _size)
			{
				GL.BufferData(BufferType, _size, data.ToArray(), BufferUsage);
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

		public IntPtr Map(BufferAccess bufferAccess = BufferAccess.WriteOnly)
		{
			Bind();
			return GL.MapBuffer(BufferType, bufferAccess);
		}

		public bool UnMap()
		{
			var retval = GL.UnmapBuffer(BufferType);
			UnBind();

			return retval;
		}

		public void Upload(List<int> data)
		{
			Elements = data.Count;
			Update(data);
		}

		public void Upload(List<Vector3> data)
		{
			Elements = data.Count;
			Update(data);
		}

		public void Upload(List<Vector2> data)
		{
			Elements = data.Count;
			Update(data);
		}

		public void UnBind()
		{
			GL.BindBuffer(BufferType, 0);
		}

		public void CleanUp()
		{
			UnBind();
			GL.DeleteBuffer(Id);
		}
		
		public void Dispose()
		{
			CleanUp();
		}
	}
}
