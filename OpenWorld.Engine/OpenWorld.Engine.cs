using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenWorld.Engine.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace OpenWorld.Engine
{
	public class OpenWorldEngine : IDisposable
	{
		GameWorldTime WorldTime = new GameWorldTime();
		Models.SkyBox skyBox;
		Models.Models Models = new Models.Models();

		Camera camera;

		public OpenWorldEngine() { }

		public void PreloadModels()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Data","Models"));
			var files = new DirectoryInfo(path).GetFiles("*.ini",SearchOption.AllDirectories).ToList();

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
					var transform = Functions.CreateTransformationMatrix(
						new Vector3(
							float.Parse(positionParts[0], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(positionParts[1], CultureInfo.InvariantCulture.NumberFormat),
							float.Parse(positionParts[2], CultureInfo.InvariantCulture.NumberFormat)),
						new Vector3(0.0f),
						new Vector3(0.01f)
						);

					transforms.Add(transform);
				}

				Models.Add(new Models.OBJLoader(name), transforms);
			}
		}

		public void Initialize(int width, int height)
		{
			WorldTime = new GameWorldTime();
			camera = new Camera(new Vector3(0.0f, 0.0f, 1.0f));
			camera.Create(width, height);
			camera.Update(0.0);

			PreloadModels();

			Models.Add(new Models.Terrain(512, 512), Functions.CreateTransformationMatrix(
				new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f), new Vector3(1.0f)));

			skyBox = new Models.SkyBox(256);

		}

		public void OnKeyDown(OpenTK.Input.Key key, float deltaTime)
		{
			switch (key)
			{
				case OpenTK.Input.Key.A:
					camera.Set_PositionX(+2.525f * deltaTime) ;
					break;
				case OpenTK.Input.Key.D:
					camera.Set_PositionX(-2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.W:
					camera.Set_PositionZ(+2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.S:
					camera.Set_PositionZ(-2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.Q:
					//camera.Set_RotationY(-2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.E:
					//camera.Set_RotationY(+2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.LShift:
					camera.Set_PositionY(+2.525f * deltaTime);
					break;
				case OpenTK.Input.Key.RShift:
					camera.Set_PositionY(-2.525f * deltaTime);
					break;
				default:
					break;
			}
		}

		public void Update(double deltatime)
		{
			WorldTime.Update();
			skyBox.Update(deltatime);
			Models.Update(deltatime);
			camera.Update(deltatime);
		}

		public void OnMouseMove(float x, float y, double deltaTime)
		{
			camera.Pitch -= y;
			camera.Yaw += x; 
		}

		public void OnResize(int width, int height)
		{
			GL.Viewport(0, 0, width, height);

			camera.Update_ProjectionMatrix(width, height);
			camera.Update_ViewMatrix();
		}

		public void Render(double deltaTime)
		{
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);

			skyBox.Draw(ref WorldTime, ref camera, true);
			Models.Draw(ref WorldTime, ref camera);

			GL.Disable(EnableCap.DepthTest);
		}

		public void Dispose()
		{
			skyBox.Dispose();
			Models.Dispose();
		}

		public void Unload()
		{
			skyBox.CleanUp();
			Models.CleanUp();
		}
	}
}
