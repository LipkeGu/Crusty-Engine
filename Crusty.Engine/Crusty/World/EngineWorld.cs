using Crusty.Engine.Common;
using Crusty.Engine.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine
{
	public class EngineWorld : IDisposable
	{
		public SkyBox Skybox { get; private set; }

		public Models.Models Models { get; private set; }

		public Terrain Terrain { get; private set; }

		public EngineWorld()
		{
			Terrain = new Terrain("Data/Texture/heightmap.png", "Data/Texture/normalmap.png");
			Skybox = new SkyBox(Terrain.Width / MathHelper.Pi);
		}

		

		public void Update(double deltatime)
		{
			Skybox.Update(deltatime);
			Terrain.Update(deltatime);
		}

		public void Render(ref GameWorldTime gameWorldTime, ref Fog fog,  ref Camera camera)
		{
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			Skybox.Draw(ref gameWorldTime, ref fog, ref camera, true);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			Terrain.Draw(ref gameWorldTime, ref fog, ref camera);

			GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.DepthTest);
		}

		public void Dispose()
		{
			Skybox.CleanUp();
			Terrain.CleanUp();
		}
	}
}
