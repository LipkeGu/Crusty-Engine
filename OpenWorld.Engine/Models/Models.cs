using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenWorld.Engine.Common;

namespace OpenWorld.Engine.Models
{

	public class Models
	{
		public Dictionary<Model, List<Matrix4>> Instances;

		public Models()
		{
			Instances = new Dictionary<Model, List<Matrix4>>();
		}

		public void Add(Model model, Matrix4 transform)
		{
			var transforms = new List<Matrix4>();
			transforms.Add(transform);

			Add(model, transforms);
		}

		public void Add(Model model, List<Matrix4> transform)
		{
			if (Instances.ContainsKey(model))
				Instances[model] = transform;
			else
				Instances.Add(model, transform);
		}

		public void Draw(ref GameWorldTime worldTime, ref Camera camera)
		{
			foreach (var instance in Instances)
			{
				GL.Enable(EnableCap.Blend);
				GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

				if (instance.Value.Count > 1)
				{
					instance.Key.VertexArray.Bind();

					instance.Key.Texture.Bind();
					instance.Key.Shader.Use();
					instance.Key.Shader.Set_Mat4("projMatrix", camera.ProjectionMatrix);
					instance.Key.Shader.Set_Mat4("viewMatrix", camera.ViewMatrix);

					instance.Key.VertexArray.Instanced = instance.Value.Count > 1;

					for (var i = 0; i < instance.Value.Count; i++)
					{
						if (instance.Key.VertexArray.Instanced)
						{
							instance.Key.Shader.Set_Mat4(string.Format("modelMatrix[{0}]", i), instance.Value[i]);
						}
					}

					GL.DrawElementsInstanced(PrimitiveType.Triangles, instance.Key.VertexArray.IndexBuffer.Elements,
					  DrawElementsType.UnsignedInt, IntPtr.Zero, instance.Value.Count);

					instance.Key.Shader.Unuse();
					instance.Key.Texture.UnBind();
					instance.Key.VertexArray.UnBind();
				}
				else
				{
					instance.Key.Draw(ref worldTime, ref camera);
				}

				GL.Disable(EnableCap.Blend);

			}
		}

		internal void Dispose()
		{
			foreach (var model in Instances)
			{
				model.Key.Dispose();
			}
		}

		internal void CleanUp()
		{
			foreach (var model in Instances)
			{
				model.Key.CleanUp();
			}

			Instances.Clear();
		}

		internal void Update(double deltatime)
		{
			foreach (var model in Instances)
			{
				model.Key.Update(deltatime);
			}
		}
	}
}
