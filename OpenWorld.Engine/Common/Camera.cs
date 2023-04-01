using OpenTK;
using OpenWorld.Engine.Common;
using OpenWorld.Engine.Models;
using OpenWorld.Engine.Traits;
using System;
namespace OpenWorld.Engine
{
	public class Camera : MoveAble
	{
		private Vector3 _front = -Vector3.UnitZ;
		private Vector3 _up = Vector3.UnitY;
		private Vector3 _right = Vector3.UnitX;
		private float _yaw = 90.0f;
		private float _pitch = 0.0f;

		public float Pitch
		{
			get { return MathHelper.RadiansToDegrees(_pitch); }
			set { _pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value, -45.0f, 45.0f)); }
		}

		public float Yaw
		{
			get { return MathHelper.RadiansToDegrees(_yaw); }
			set { _yaw = MathHelper.DegreesToRadians(value); }
		}

		private float _fov = 75.0f;
		private float far = 1000.0f;

		public Matrix4 ProjectionMatrix { get; private set; } = Matrix4.Identity;
		public Matrix4 ViewMatrix { get; private set; } = Matrix4.Identity;

		public void Update_ProjectionMatrix(int width, int height, float far)
		{
			if (width == 0 || height == 0)
				return;

			ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), width / height, 0.01f, 1000.0f);
		}



		public Camera(Vector3 position)
		{
			Position = position;
		}

		public void Create(int width, int height, float far)
		{
			this.far = far;

			if (width == 0 || height == 0)
				return;

			Update_ProjectionMatrix(width, height, far);
		}

		public void OnResize(int width, int height)
		{
			if (width == 0 || height == 0)
				return;

			Update_ProjectionMatrix(width, height, far);
		}

		public void Update(Terrain terrain, double deltatime)
		{
			Position.Y = terrain.GetHeightAt((int)Position.X, (int)Position.Z) + 6;
			_front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
			_front.Y = (float)Math.Sin(_pitch);
			_front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
			_front = Vector3.Normalize(_front);
			_right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
			_up = Vector3.Normalize(Vector3.Cross(_right, _front));

			ViewMatrix = Functions.Update_ViewMatrix(Position, Rotation, _front, _up);
		}
	}
}
