using OpenTK;
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

		public float Yaw {
			get { return MathHelper.RadiansToDegrees(_yaw); }
			set { _yaw = MathHelper.DegreesToRadians(value); } }

		private float _fov = 90.0f;

		public Matrix4 ProjectionMatrix { get; private set; } = Matrix4.Identity;
		public Matrix4 ViewMatrix { get; private set; } = Matrix4.Identity;

		public void Update_ProjectionMatrix(int width, int height)
		{
			if (width == 0 || height == 0)
				return;

			ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), width / height, 0.01f, 1000.0f);
		}

		public void Update_ViewMatrix()
		{
			ViewMatrix = Matrix4.LookAt(Position, Position + _front, _up);
			ViewMatrix *= Matrix4.CreateRotationX(Rotation.X);
			ViewMatrix *= Matrix4.CreateRotationY(Rotation.Y);
			ViewMatrix *= Matrix4.CreateRotationZ(Rotation.Z);
		}

		public Camera(Vector3 position)
		{
			Position = position;
		}

		public void Create(int width, int height)
		{
			if (width == 0 || height == 0)
				return;

			Update_ProjectionMatrix(width, height);
		}

		public void Update(double deltatime)
		{
			_front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
			_front.Y = (float)Math.Sin(_pitch);
			_front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
			_front = Vector3.Normalize(_front);
			_right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
			_up = Vector3.Normalize(Vector3.Cross(_right, _front));
		
			Update_ViewMatrix();
		}
	}
}
