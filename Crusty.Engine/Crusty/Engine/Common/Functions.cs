using OpenTK;
using System;

namespace Crusty.Engine.Common
{
	public static class Functions
	{
		public static string[] SplitString(this string str, string delimeter)
		{
			return str.Split(delimeter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}

		public static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
		{
			float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
			float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
			float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
			float l3 = 1.0f - l1 - l2;

			return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
		}

		public static Matrix4 Update_ViewMatrix(Vector3 position, Vector3 rotation, Vector3 front, Vector3 up)
		{
			var ViewMatrix = Matrix4.LookAt(position, position + front, up);
			ViewMatrix *= Matrix4.CreateRotationX(rotation.X);
			ViewMatrix *= Matrix4.CreateRotationY(rotation.Y);
			ViewMatrix *= Matrix4.CreateRotationZ(rotation.Z);

			return ViewMatrix;
		}

		public static Matrix4 CreateTransformationMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			var modelMatrix = Matrix4.Identity;
			modelMatrix *= Matrix4.CreateTranslation(position);
			modelMatrix *= Matrix4.CreateRotationX(rotation.X);
			modelMatrix *= Matrix4.CreateRotationY(rotation.Y);
			modelMatrix *= Matrix4.CreateRotationZ(rotation.Z);
			modelMatrix *= Matrix4.CreateScale(scale);
			return modelMatrix;
		}
	}
}
