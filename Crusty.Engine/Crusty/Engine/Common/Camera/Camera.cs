using OpenTK;
using Crusty.Engine.Common;
using Crusty.Engine.Models;
using System;
using OpenTK.Input;
using Crusty.Engine.Common.Camera;
using Crusty.Engine.Common.Traits;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace Crusty.Engine
{
	public class Camera : MoveAble, ICamera 
	{
		Vector3 front = -Vector3.UnitZ;
		private float _yaw = 90.0f;
		private float _pitch = 0.0f;

		int width = 0;
		int height = 0;

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
		}

		public void OnResize(int width, int height, float far)
		{
			this.width = width;
			this.height = height;

			GL.Viewport(new Rectangle(0, 0, width, height));

			ProjectionMatrix = Functions.Update_ProjectionMatrix(width, height, far + 500);
		}

		public void Update(Terrain terrain, double deltatime)
		{
			Position.Y = terrain.QueryHeightAt((int)Position.X, (int)Position.Z) + 6;

			front = Functions.CalculateFront(_pitch, _yaw);

			ViewMatrix = Functions.Update_ViewMatrix(Position, Rotation, front, Vector3.UnitY);
			RayPosition = calculateMouseRay();
		}

		Vector3 calculateMouseRay()
		{
			float mouseX = Mouse.GetState().X;
			float mouseY = Mouse.GetState().Y; ;

			var NomalizeddeviceCoords = GetNormalizedDeviceCoords(mouseX, mouseY);
			var ClipCoords = new Vector4(NomalizeddeviceCoords.X, NomalizeddeviceCoords.Y, -1, 1);
			var eyeCoords = ToEyeCoords(ClipCoords);

			return toWorldCoords(eyeCoords);

		}

		Vector2 GetNormalizedDeviceCoords(float mouseX, float mouseY, bool flipY = false)
		{
			float x = (2 * mouseX) / (width - 1);
			float y = (2 * mouseY) / (height - 1);

			return new Vector2(x, flipY ? y : -y);
		}

		Vector4 ToEyeCoords(Vector4 clipCoords)
		{
			Matrix4 inverseProjMatrix = ProjectionMatrix.Inverted();
			Vector4 eyeCoords = Vector4.Transform(inverseProjMatrix, clipCoords);

			return new Vector4(eyeCoords.X, eyeCoords.Y, -1, 0f);
		}

		Vector3 toWorldCoords(Vector4 eyeCoords)
		{
			var inversViewMatrix = ViewMatrix.Inverted();
			var worldCoords = Vector4.Transform(inversViewMatrix, eyeCoords);

			return new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z).Normalized();
		}
	}
}
