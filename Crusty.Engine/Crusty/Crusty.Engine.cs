﻿using OpenTK;
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

namespace Crusty.Engine
{
	public class CrustyEngine : IDisposable
	{
		public Input Input { get; private set; } = new Input();
		GameWorldTime WorldTime = new GameWorldTime();
		EngineWorld EngineWorld;
		bool TerainDebug = false;

		Models.Models Models = new Models.Models();
		float deltaTime = 0.0f;

		Camera camera;
		Fog fog;

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

		public void OnKeyUp(Key key)
		{
			Input.SetState(key, false);
		}

		public void Initialize(int width, int height)
		{
			Input.InputKeyDown += (sender, e) =>
			{
				var camSpeed = 5.525f;
				switch (e.Key)
				{
					case Key.W:
						camera.Move_Forward(camSpeed * deltaTime);
						break;
					case Key.S:
						camera.Move_Backward(camSpeed / (camSpeed / 2) * deltaTime);
						break;
					case Key.A:
						camera.Move_Left((camSpeed / 1.5f) * deltaTime);
						break;
					case Key.D:
						camera.Move_Right((camSpeed / 1.5f) * deltaTime);
						break;
					case Key.F7:
						if (TerainDebug)
							TerainDebug = false;
						else
							TerainDebug = true;
						break;
				}
			};


			Input.Initialize();
			EngineWorld = new EngineWorld();
			WorldTime = new GameWorldTime();
			camera = new Camera(new Vector3(EngineWorld.Terrain.Width / 2, 6.0f, EngineWorld.Terrain.Height / 2));
			camera.Create(width, height,EngineWorld.Skybox.Size *3);
			fog = new Fog();
		}

		public void OnKeyDown(OpenTK.Input.Key key, float deltaTime)
		{
			this.deltaTime = deltaTime;
			Input.SetState(key, true);
		}

		public void Update(double deltatime)
		{
			Input.Update(deltatime);
			camera.Update(EngineWorld.Terrain, deltatime);
			EngineWorld.Update(deltatime);
			WorldTime.Update();
			Models.Update(deltatime);

		}

		public void OnMouseMove(CursorPosition cursorPosition, double deltaTime)
		{
			Input.SetMousePosition(cursorPosition);
			camera.Pitch -= cursorPosition.Y;
			camera.Yaw += cursorPosition.X;
		}

		public void OnMouseDown(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, pressed);
		}

		public void OnMouseUp(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, pressed);
		}

		public void OnResize(int width, int height)
		{
			GL.Viewport(new Rectangle(0, 0, width, height));
			camera.OnResize(width, height);
		}

		public void Render(double deltaTime)
		{
			EngineWorld.Render(ref WorldTime, ref fog, ref camera);
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
