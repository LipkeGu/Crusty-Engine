using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Crusty.Engine.Video
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
		/// Anzahl der Elemente im Buffer.
		/// </summary>
		public int Elements { get; private set; } = 0;

		/// <summary>
		/// Typ des Buffers... (Array, Element, etc...)
		/// </summary>
		public BufferTarget BufferType { get; private set; } = BufferTarget.ArrayBuffer;

		public Buffer(BufferTarget target)
		{
			BufferType = target;
		}

		/// <summary>
		/// Erzeugt  und bindet einen Buffer.
		/// </summary>
		public void Create()
		{
			Id = GL.GenBuffer();
			Bind();
		}

		/// <summary>
		/// Aktiviert den Buffer für die Verwendung.
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
		public void Upload(List<int> data, BufferUsageHint usage = BufferUsageHint.StaticDraw)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Marshal.SizeOf(data[0]), data.ToArray(), usage);
			Elements = data.Count;
			UnBind();
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector3> data, BufferUsageHint usage = BufferUsageHint.StaticDraw)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Marshal.SizeOf(data[0]), data.ToArray(), usage);
			Elements = data.Count;
			UnBind();
		}

		/// <summary>
		/// Lädt Daten in den Buffer.
		/// </summary>
		/// <param name="data">Die Daten...</param>
		/// <param name="usage">Verwendungsart... (Default: StaticDraw).</param>
		public void Upload(List<Vector2> data, BufferUsageHint usage = BufferUsageHint.StaticDraw)
		{
			Bind();
			GL.BufferData(BufferType, data.Count * Marshal.SizeOf(data[0]), data.ToArray(), usage);
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
