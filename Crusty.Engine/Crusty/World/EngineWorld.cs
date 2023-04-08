using Crusty.Engine.Common;
using Crusty.Engine.Common.Camera;
using Crusty.Engine.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Crusty.Engine
{
	public class EngineWorld : IDisposable
	{
		public SkyBox Skybox { get; private set; }

		Fog fog = new Fog();

		public Models.Models Models { get; private set; } = new Models.Models();

		IList<Light> Lights;

		public Terrain Terrain { get; private set; }

		public EngineWorld()
		{
			Terrain = new Terrain("Data/Texture/heightmap.png", "Data/Texture/normalmap.png");
			Skybox = new SkyBox(Terrain.Width);
			
			Lights = new List<Light>
			{
				new Light(new Vector3(Terrain.Width / 2, (Skybox.Size) / 2, Terrain.Height / 2), new Vector3(1.0f)),
				new Light(new Vector3(0, (Skybox.Size)  / 2 , 0), new Vector3(1.0f))
			};




			var rand = new Random();

			

			for (int i = 0; i < 1000; i++)
			{
				var x = rand.Next(-Terrain.Width, Terrain.Width);
				var z = rand.Next(-Terrain.Height, Terrain.Height);
				var scale = new Vector3(0.2f);
				var rot = new Vector3(x, 0, z);
				var pos = new Vector3(x, 0, z);

				var transform = Functions.CreateTransformationMatrix(pos, rot, scale);

				Models.Add(new Quad("Sun", 10, pos, rot, scale), transform);
			}

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Disable(EnableCap.CullFace);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.Disable(EnableCap.DepthTest);
		}

		public void Update(double deltatime)
		{
			Skybox.Update(null, deltatime);
			Terrain.Update(null, deltatime);

			Models.Update(Terrain, deltatime);

			foreach (var light in Lights)
				light.Update(deltatime);
		}

		public bool IsInWorld(float axe)
		{
			var width = Terrain.Width;
			var height = Terrain.Height;
		
			return ((axe < width && width > axe) && (height > axe && axe < height));
		}

		public void Render(double deltaTime, ref GameWorldTime gameWorldTime, ref ICamera camera)
		{
			GL.Enable(EnableCap.DepthTest);
			Skybox.Draw(ref gameWorldTime, ref Lights, ref fog, camera.ProjectionMatrix, camera.ViewMatrix, true);

			GL.Enable(EnableCap.CullFace);
			Terrain.Draw(ref gameWorldTime, ref Lights, ref fog, camera.ProjectionMatrix, camera.ViewMatrix);
			GL.Disable(EnableCap.CullFace);

			foreach (var light in Lights)
				light.Draw(ref gameWorldTime, ref Lights, ref fog, camera.ProjectionMatrix, camera.ViewMatrix, false);

			Models.Draw(ref gameWorldTime, ref Lights, ref fog, ref camera);

			GL.Disable(EnableCap.DepthTest);
		}

		public void Dispose()
		{
			Skybox.CleanUp();
			Terrain.CleanUp();
		}
	}
}
