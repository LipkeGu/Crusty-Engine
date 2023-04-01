using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenWorld.Engine.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using OpenWorld.Engine.Models;

namespace OpenWorld.Engine
{
	public class OpenWorldEngine : IDisposable
	{
		GameWorldTime WorldTime = new GameWorldTime();
		Models.SkyBox skyBox;
		Models.Terrain terrain;

		bool TerainDebug = false;

		Models.Models Models = new Models.Models();

		Camera camera;
		Light sun;

		MousePicker picker;

		public void PreloadModels(ref Terrain terrain)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Data", "Models"));
			var files = new DirectoryInfo(path).GetFiles("*.ini", SearchOption.AllDirectories).ToList();

			for (var i = 0; i < files.Count; i++)
			{
				var modelInfo = new Ini.IniFile(files[i].FullName);
				var transforms = new List<Matrix4>();
				var name = modelInfo.IniReadValue("Global", "Name");
				var num_Pos = modelInfo.IniReadValue("Global", "Positions");
				var numPositions = int.Parse(num_Pos);

				for (var iP = 0; iP < numPositions; iP++)
				{
					var positionParts = Functions.SplitString(modelInfo.IniReadValue("Positions", string.Format("Position{0}", iP)), ";");
					var modelPosition = new Vector3(
							float.Parse(positionParts[0], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(positionParts[1], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(positionParts[2], CultureInfo.InvariantCulture.NumberFormat));

					modelPosition.Y = terrain.GetHeightAt((int)modelPosition.X, (int)modelPosition.Z);

					var transform = Functions.CreateTransformationMatrix(modelPosition,
						new Vector3(0.0f),
						new Vector3(10)
						);

					transforms.Add(transform);
				}

				Models.Add(new Models.OBJLoader(name), transforms);
			}
		}

		public void Initialize(int width, int height)
		{
			WorldTime = new GameWorldTime();
			terrain = new Models.Terrain("Data/Texture/heightmap.png", "Data/Texture/normalmap.png");
			skyBox = new Models.SkyBox(terrain.Width / MathHelper.Pi);
			camera = new Camera(new Vector3(terrain.Width / 2, 6.0f, terrain.Height / 2));
			camera.Create(width, height, skyBox.Size * 2);
			camera.Update(terrain, 0.0);
			picker = new MousePicker(camera);
			sun = new Light(new Vector3(terrain.Width / 2, 100.0f, terrain.Height / 2), new Vector3(255), 10.0f);
			//PreloadModels(ref terrain);
		}

		public void OnKeyDown(OpenTK.Input.Key key, float deltaTime)
		{
			switch (key)
			{
				case OpenTK.Input.Key.A:
					if (terrain.IsInsideBounds(camera.Position.X + 1.0f))
						camera.Set_PositionX(+20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.D:
					if (terrain.IsInsideBounds(camera.Position.X - 1.0f))
						camera.Set_PositionX(-20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.W:
					if (terrain.IsInsideBounds(camera.Position.Z + 1.0f))
						camera.Set_PositionZ(+20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.S:
					if (terrain.IsInsideBounds(camera.Position.Z - 1.0f))
						camera.Set_PositionZ(-20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.Q:
					//camera.Set_RotationY(-20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.E:
					//camera.Set_RotationY(+20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.LShift:
					camera.Set_PositionY(+20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.RShift:
					camera.Set_PositionY(-20.525f * deltaTime);
					break;
				case OpenTK.Input.Key.F7:
					if (TerainDebug)
						TerainDebug = false;
					else
						TerainDebug = true;
					break;
				default:
					break;
			}
		}

		public void Update(int width, int height, double deltatime)
		{
			WorldTime.Update();
			skyBox.Update(deltatime, false);
			terrain.Update(deltatime, TerainDebug);
			Models.Update(deltatime);
			camera.Update(terrain, deltatime);
			picker.Update(width, height);

		}

		public void OnMouseMove(float x, float y, double deltaTime)
		{
			camera.Pitch -= y;
			camera.Yaw += x;
		}

		public void OnResize(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			camera.OnResize(width, height);
		}

		public void Render(double deltaTime)
		{

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);

			skyBox.Draw(ref WorldTime, ref camera, true);

			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			terrain.Draw(ref WorldTime, ref camera);
			GL.Disable(EnableCap.CullFace);

			Models.Draw(ref WorldTime, ref camera);
			GL.Disable(EnableCap.DepthTest);
		}

		public void Dispose()
		{
			skyBox.Dispose();
			terrain.Dispose();
			Models.Dispose();
		}

		public void Unload()
		{
			skyBox.CleanUp();
			terrain.Dispose();
			Models.CleanUp();
		}
	}
}
