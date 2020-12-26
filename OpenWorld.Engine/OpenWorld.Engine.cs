using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenWorld.Engine.Common;
namespace OpenWorld.Engine
{
	public class OpenWorldEngine : IDisposable
	{
		GameWorldTime WorldTime = new GameWorldTime();
		Models.Terrain terrain;
		Models.SkyBox skyBox;
		Camera camera;

		public OpenWorldEngine() { }

		public void Initialize(int width, int height)
		{
			WorldTime = new GameWorldTime();
			camera = new Camera(new Vector3(1.0f, 10.0f, -1.0f));
			camera.Create(width, height);
			camera.Update(0.0);

			terrain = new Models.Terrain(1024,1024);
			skyBox = new Models.SkyBox(1024);
			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
		}

		public void OnKeyDown(OpenTK.Input.Key key)
		{
			switch (key)
			{
				case OpenTK.Input.Key.A:
					camera.Set_PositionX(+0.025f);
					break;
				case OpenTK.Input.Key.D:
					camera.Set_PositionX(-0.025f);
					break;
				case OpenTK.Input.Key.W:
					camera.Set_PositionZ(+0.025f);
					break;
				case OpenTK.Input.Key.S:
					camera.Set_PositionZ(-0.025f);
					break;
				case OpenTK.Input.Key.Q:
					camera.Set_RotationY(-0.025f);
					break;
				case OpenTK.Input.Key.E:
					camera.Set_RotationY(+0.025f);
					break;
				case OpenTK.Input.Key.LShift:
					camera.Set_PositionY(+0.025f);
					break;
				case OpenTK.Input.Key.RShift:
					camera.Set_PositionY(-0.025f);
					break;
				default:
					break;
			}

		}

		public void Update(double deltatime)
		{
			WorldTime.Update();
			terrain.Update(deltatime);
			skyBox.Update(deltatime);

			camera.Update(deltatime);
		}

		public void OnMouseMove(float x, float y)
		{
			//camera.Pitch -= y;
			//camera.Yaw += x; 
		}

		public void OnResize(int width, int height)
		{
			GL.Viewport(0, 0, width, height);

			camera.Update_ProjectionMatrix(width, height);
			camera.Update_ViewMatrix();
		}

		public void Render()
		{
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			terrain.Draw(ref WorldTime, ref camera);
			skyBox.Draw(ref WorldTime, ref camera, true);
			GL.Disable(EnableCap.DepthTest);
		}

		public void Dispose()
		{
			terrain.Dispose();
			skyBox.Dispose();
		}

		public void Unload()
		{
			terrain.CleanUp();
			skyBox.CleanUp();
		}
	}
}
