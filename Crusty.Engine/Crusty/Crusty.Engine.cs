using OpenTK;
using System;
using Crusty.Engine.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crusty.Engine.Models;
using OpenTK.Input;
using Crusty.Engine.Common.Camera;
using Crusty.Engine.Crusty.Models.Interface;
using OpenTK.Graphics.OpenGL;

namespace Crusty.Engine
{
	public class CrustyEngine : IDisposable
	{
		public Input Input { get; private set; } = new Input();
		public Video Video { get; private set; } = new Video();

		public bool Enabled = true;

		GameWorldTime WorldTime = new GameWorldTime();
		EngineWorld EngineWorld;

		Models.Models Models = new Models.Models();

		public ICamera Camera;

		public void PreloadModels(ref ITerrain terrain)
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
					var positionParts = modelInfo.IniReadValue("Positions", string.Format("Position{0}", iP)).Split(";");
					var modelPosition = Functions.CreateVec3(positionParts[0], positionParts[1], positionParts[2]);

					modelPosition.Y = terrain.QueryHeightAt((int)modelPosition.X, (int)modelPosition.Z);

					var transform = Functions.CreateTransformationMatrix(modelPosition,
						new Vector3(0.0f), new Vector3(1));

					transforms.Add(transform);
				}

				Models.Add(new OBJLoader(name), transforms);
			}
		}

		public void OnKeyUp(Key key, bool altPressed, bool shiftPressed)
		{
			Input.SetState(key, false, altPressed, shiftPressed);
		}

		public void Initialize(int width, int height)
		{
			Input.InputMouseButtonDown += (sender, e) => { };

			Input.InputKeyDown += (sender, e) =>
			{
				var camSpeed = e.ShiftPressed ? 90.525f : 50.525f;
				if (!e.Pressed)
					return;

				switch (e.Key)
				{
					case Key.W:
						Camera.Move_Forward(camSpeed * e.DeltaTIme);
						break;
					case Key.S:
						Camera.Move_Backward(camSpeed / (camSpeed / 2) * e.DeltaTIme);
						break;
					case Key.A:
						Camera.Move_Left((camSpeed / 1.5f) * e.DeltaTIme);
						break;
					case Key.D:
						Camera.Move_Right((camSpeed / 1.5f) * e.DeltaTIme);
						break;
					default:
						break;
				}
			};

			Input.Initialize();
			EngineWorld = new EngineWorld();
			WorldTime = new GameWorldTime();

			Camera = new Camera(new Vector3(EngineWorld.Terrain.Width / 2, 6.0f, EngineWorld.Terrain.Height / 2));
			Camera.OnResize(width, height, EngineWorld.Skybox.Size * 1.5f);
		}

		public void OnKeyDown(Key key, bool altPressed, bool shiftPressed)
		{
			Input.SetState(key, true, altPressed, shiftPressed);
		}

		public void Update(double deltatime)
		{
			Input.Update(deltatime);
			Camera.Update(EngineWorld.Terrain, deltatime);
			EngineWorld.Update(deltatime);
			WorldTime.Update();

			Models.Update(EngineWorld.Terrain, deltatime);
		}

		public void OnMouseMove(CursorPosition cursorPosition)
		{
			Camera.OnMouseMove(cursorPosition);
			Camera.RayPosition = Functions.UnProject(cursorPosition, ref Camera);
			Input.SetMousePosition(Camera.RayPosition, cursorPosition);
		}

		public void OnMouseDown(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, Camera.RayPosition, pressed);
		}

		public void OnMouseUp(bool pressed, CursorPosition position, MouseButton button)
		{
			Input.SetState(button, position, Camera.RayPosition, pressed);
		}

		public void OnResize(int width, int height)
		{
			Camera.OnResize(width, height, EngineWorld.Skybox.Size * 1.5f);
		}

		public void Render(double deltaTime)
		{
			Video.BeginRender();

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.StencilTest);

			if (Enabled)
				EngineWorld.Render(deltaTime, ref WorldTime, ref Camera);

			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.StencilTest);
		}

		public void Dispose()
		{
			EngineWorld.Dispose();
			Models.Dispose();
		}

		public void OnUnload()
		{
			Input.Stop();
			Models.CleanUp();
		}
	}
}
