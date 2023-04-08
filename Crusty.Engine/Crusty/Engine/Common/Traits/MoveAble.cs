using OpenTK;

namespace Crusty.Engine.Common.Traits
{
	public class MoveAble
	{
		public Vector3 Position = new Vector3();
		public Vector3 Rotation = new Vector3();
		public Vector3 Scale = new Vector3(1.0f);

		public Matrix4 ModelMatrix { get; private set; }

		public virtual void Move_Forward(Vector3 front, float speed)
		{
			Position += front * speed;
		}

		public void UpdateModelMatrix()
		{
			ModelMatrix = Functions.CreateTransformationMatrix(Position, Rotation, Scale);
		}

		public virtual void Move_Backward(Vector3 front, float speed)
		{
			Position -= front * speed;
		}

		public virtual void Move_Left(Vector3 front, Vector3 up, float speed)
		{
			Position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed;
		}

		public virtual void Move_Right(Vector3 front, Vector3 up, float speed)
		{
			Position += Vector3.Normalize(Vector3.Cross(front, up)) * speed;
		}
	}
}
