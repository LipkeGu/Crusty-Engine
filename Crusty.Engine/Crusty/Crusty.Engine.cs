using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using Crusty.Engine.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using Crusty.Engine.Models;
using OpenTK.Input;
using System.Drawing;
using Crusty.Engine.System;

namespace Crusty.Engine
{
	public class CrustyEngine : IDisposable
	{
		public Input Input { get; private set; } = new Input();
		public Video Video { get; private set; } = new Video();

		GameWorldTime WorldTime = new GameWorldTime();
		EngineWorld EngineWorld;
		bool TerainDebug = false;

		Models.Models Models = new Models.Models();
		

		
		Camera camera;
		Fog fog;
		List<Light> Lights = new List<Light>();

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

		public void OnKeyUp(Key key, bool altPressed, bool shiftPressed)
		{
			Input.SetState(key, false,altPressed, shiftPressed);
		}

		public void Initialize(int width, int height)
		{

			Input.InputMouseButtonDown += (sender, e) =>
			{
				switch (e.Button)
				{
					default:
						break;
				}
			};

			Input.InputKeyDown += (sender, e) =>
			{
				var camSpeed = e.ShiftPressed ? 9.525f : 5.525f;
				if (!e.Pressed)
					return;

				switch (e.Key)
				{
					case Key.W:
						camera.Move_Forward(camSpeed * e.DeltaTIme);
						break;
					case Key.S:
						camera.Move_Backward(camSpeed / (camSpeed / 2) * e.DeltaTIme);
						break;
					case Key.A:
						camera.Move_Left((camSpeed / 1.5f) * e.DeltaTIme);
						break;
					case Key.D:
						camera.Move_Right((camSpeed / 1.5f) * e.DeltaTIme);
						break;
					case Key.F7:
						TerainDebug = TerainDebug ? false : true;
						break;
					case Key.F8:
						camera.FlyMode = camera.FlyMode ? false : true;
						break;
					default:
						break;
				}
			};

			Input.Initialize();
			EngineWorld = new EngineWorld();
			WorldTime = new GameWorldTime();
			camera = new Camera(new Vector3(EngineWorld.Terrain.Width / 2, 6.0f, EngineWorld.Terrain.Height / 2));
			camera.Create(width, height, EngineWorld.Skybox.Size * 3);
			fog = new Fog();
			Lights.Add(new Light(new Vector3(0, 1000, 0), WorldTime.LightColor));
		}

		public void OnKeyDown(Key key, bool altPressed, bool shiftPressed)
		{
			Input.SetState(key, true, altPressed, shiftPressed);
		}

		public void Update(double deltatime)
		{
			Input.Update(deltatime);
			camera.Update(EngineWorld.Terrain, deltatime);
			EngineWorld.Update(deltatime);
			WorldTime.Update();

			Models.Update(deltatime);
		}

		public void OnMouseMove(CursorPosition cursorPosition)
		{
			
			camera.Pitch -= cursorPosition.Y;
			camera.Yaw += cursorPosition.X;
			Input.SetMousePosition(camera.CurrentRay, cursorPosition);
		}

		public void OnMouseDown(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, camera.CurrentRay, pressed);
		}

		public void OnMouseUp(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, camera.CurrentRay, pressed);
		}

		public void OnResize(int width, int height)
		{
			GL.Viewport(new Rectangle(0, 0, width, height));
			camera.OnResize(width, height);
		}

		public void Render(double deltaTime)
		{
			EngineWorld.Render(ref WorldTime, ref Lights, ref fog, ref camera);
		}

		public void Dispose()
		{
			EngineWorld.Dispose();
			Models.Dispose();
		}

		public void Unload()
		{
			Models.CleanUp();
		}
	}
}
