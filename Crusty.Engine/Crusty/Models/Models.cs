using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Crusty.Engine.Common;

namespace Crusty.Engine.Models
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

		public void Draw(ref GameWorldTime worldTime, ref List<Light> light, ref Fog fog, ref Camera camera)
		{
			foreach (var instance in Instances)
			{
				if (instance.Value.Count > 1)
				{
					instance.Key.VertexArray.Bind();

					instance.Key.Texture.Bind();
					instance.Key.Shader.Use();
					instance.Key.Shader.Set_Mat4("projMatrix", camera.ProjectionMatrix);
					instance.Key.Shader.Set_Mat4("viewMatrix", camera.ViewMatrix);
					instance.Key.Shader.Set_Shine_Variables(instance.Key.shineDamper, instance.Key.reflectivity);
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
					instance.Key.Draw(ref worldTime, ref light, ref fog, ref camera);
				}
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
				model.Key.Update(deltatime, false);
			}
		}
	}
}
