using OpenTK;
using Crusty.Engine.Common;
using Crusty.Engine.Models;
using System;
using OpenTK.Input;
using Crusty.Engine.Common.Camera;
using Crusty.Engine.Common.Traits;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using Crusty.Engine.Crusty.Models.Interface;

namespace Crusty.Engine
{
	public class Camera : MoveAble, ICamera 
	{
		Vector3 front = -Vector3.UnitZ;
		private float _yaw = 90.0f;
		private float _pitch = 0.0f;

		int Width = 0;
		int Height = 0;

		public bool FlyMode {get; set;} = false;


		public float Pitch
		{
			get { return MathHelper.RadiansToDegrees(_pitch); }
			set { _pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value, -45.0f, 45.0f)); }
		}

		Vector3 cameraTarget = Vector3.Zero;
		Vector3 cameraDirection;

		public float Yaw
		{
			get { return MathHelper.RadiansToDegrees(_yaw); }
			set { _yaw = MathHelper.DegreesToRadians( value); }
		}

		public Matrix4 ProjectionMatrix { get; set; } = Matrix4.Identity;

		public Matrix4 ViewMatrix { get; set; } = Matrix4.Identity;

		public Vector3 RayPosition { get; set; }

		public float Far { get; set; } = 1000.0f;
		public float Near { get; set; } = 0.01f;

		public Camera(Vector3 position)
		{
			Position = position;
			cameraDirection = Vector3.Normalize(Position - cameraTarget);
		}

		public virtual void Move_Forward(float speed)
		{
			Move_Forward(front, speed);
		}

		public virtual void Move_Backward(float speed)
		{
			Move_Backward(front, speed);
		}

		public virtual void Move_Left(float speed)
		{
			Move_Left(front, Vector3.UnitY, speed);
		}

		public virtual void Move_Right(float speed)
		{
			Move_Right(front, Vector3.UnitY, speed);
		}

		public void OnMouseMove(CursorPosition cursorPosition)
		{
			Pitch -= cursorPosition.Y;
			Yaw += cursorPosition.X;

			RayPosition = calculateMouseRay(cursorPosition.X, cursorPosition.Y);
		}

		public void OnResize(int width, int height, float far)
		{
			Far = far + 500;
			Width = width;
			Height = height;
			ProjectionMatrix = Functions.Update_ProjectionMatrix(Width, Height, Near, Far);
		}

		public void Update(ITerrain terrain, double deltatime)
		{
			Position.Y = terrain.QueryHeightAt((int)Position.X, (int)Position.Z) + 6;

			front = Functions.CalculateFront(_pitch, _yaw);
			ViewMatrix = Functions.Update_ViewMatrix(Position, Rotation, front, Vector3.UnitY);
		}

		Vector3 calculateMouseRay(int mouseX, int mouseY)
		{
			var NomalizeddeviceCoords = GetNormalizedDeviceCoords(mouseX, mouseY);
			var ClipCoords = new Vector4(NomalizeddeviceCoords.X, NomalizeddeviceCoords.Y, -1, 1);
			var eyeCoords = ToEyeCoords(ClipCoords);

			return toWorldCoords(eyeCoords);
		}

		Vector2 GetNormalizedDeviceCoords(float mouseX, float mouseY)
		{
			float x = (2 * mouseX) / (Width - 1);
			float y = (2 * mouseY) / (Height - 1);

			return new Vector2(x, -y);
		}

		Vector4 ToEyeCoords(Vector4 clipCoords)
		{
			var eyeCoords = Vector4.Transform(ProjectionMatrix.Inverted(), clipCoords);
			return new Vector4(eyeCoords.X, eyeCoords.Y, -1, 0f);
		}

		Vector3 toWorldCoords(Vector4 eyeCoords)
		{
			var worldCoords = Vector4.Transform(ViewMatrix.Inverted(), eyeCoords);

			return new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z).Normalized();
		}
	}
}
