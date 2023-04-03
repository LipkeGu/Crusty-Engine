using OpenTK;
using Crusty.Engine.Common;
using Crusty.Engine.Models;
using Crusty.Engine.Traits;
using System;
using OpenTK.Input;

namespace Crusty.Engine
{
	public class Camera
	{


		private Vector3 _front = -Vector3.UnitZ;
		private Vector3 _right = Vector3.UnitX;
		private Vector3 _up = Vector3.UnitY;
		private float _yaw = 90.0f;
		private float _pitch = 0.0f;
		public Vector3 CurrentRay;

		public Vector3 Position = new Vector3(0.0f, 6.0f, 0.0f);
		public Vector3 Rotation = new Vector3(0.0f, 0.0f, 0.0f);

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

		private float _fov = 75.0f;
		private float far = 1000.0f;

		public Matrix4 ProjectionMatrix { get; private set; } = Matrix4.Identity;
		public Matrix4 ViewMatrix { get; private set; } = Matrix4.Identity;

		public void Update_ProjectionMatrix(int width, int height, float far = 1000.0f)
		{
			if (width == 0 || height == 0)
				return;
			this.far = far;

			ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), width / height, 0.01f, this.far);
		}

		public Camera(Vector3 position)
		{
			Position = position;
			cameraDirection = Vector3.Normalize(Position - cameraTarget);
		}


		public void Create(int width, int height, float far)
		{
			if (width == 0 || height == 0)
				return;

			this.width = width;
			this.height = height;

			this.far = far;

			OnResize(width, height);
		}

		public void OnResize(int width, int height)
		{
			this.width = width;
			this.height = height;

			Update_ProjectionMatrix(width, height, far);
		}

		public void Update(Terrain terrain, double deltatime)
		{
			if (!FlyMode)
				Position.Y = terrain.GetHeightAt((int)Position.X, (int)Position.Z) + 6;
			
			_front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
			_front.Y = (float)Math.Sin(_pitch);
			_front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
			_front = Vector3.Normalize(_front);


			ViewMatrix = Functions.Update_ViewMatrix(Position, Rotation, _front, _up);
			CurrentRay = calculateMouseRay();
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


		public void Move_Forward(float speed)
		{
			Position += _front * speed;
		}

		public void Move_Backward(float speed)
		{
			Position -= _front * speed;
		}

		public void Move_Left(float speed)
		{
			Position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * speed; //Left
		}

		public void Move_Right(float speed)
		{
			Position += Vector3.Normalize(Vector3.Cross(_front, _up)) * speed; //Left
		}

	}
}
