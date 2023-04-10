using OpenTK;
using System;
using System.Globalization;

namespace Crusty.Engine.Common
{
	public static class Functions
	{
		public static string[] Split(this string str, string delimeter)
			=> str.Split(delimeter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

		public static Vector3 CreateVec3(string x, string y, string z)
		{
			return new Vector3(float.Parse(x, CultureInfo.InvariantCulture.NumberFormat),
				float.Parse(y, CultureInfo.InvariantCulture.NumberFormat), 
				float.Parse(z, CultureInfo.InvariantCulture.NumberFormat));
		}
		
		public static Vector3 CalculateFront(float pitch, float yaw)
		{
			var _front = -Vector3.UnitZ;
			_front.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
			_front.Y = (float)Math.Sin(pitch);
			_front.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);
			_front = Vector3.Normalize(_front);

			return _front;
		}

		public static Vector2 CreateVec2(string x, string y)
		{
			return new Vector2(float.Parse(x, CultureInfo.InvariantCulture.NumberFormat),
				float.Parse(y, CultureInfo.InvariantCulture.NumberFormat));
		}

		public static Matrix4 Update_ViewMatrix(Vector3 position, Vector3 rotation, Vector3 front, Vector3 up)
		{
			var matrix = Matrix4.LookAt(position, position + front, up);
			matrix *= Matrix4.CreateRotationX(rotation.X);
			matrix *= Matrix4.CreateRotationY(rotation.Y);
			matrix *= Matrix4.CreateRotationZ(rotation.Z);

			return matrix;
		}

		public static Matrix4 Update_ProjectionMatrix(int width, int height, float near, float far = 1000.0f, float fov = 75.0f)
		{
			return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), width / height, near, far);
		}

		public static Matrix4 CreateTransformationMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			var model = Matrix4.Identity;
			
			model *= Matrix4.CreateTranslation(position);
			model *= Matrix4.CreateRotationX(rotation.X);
			model *= Matrix4.CreateRotationY(rotation.Y);
			model *= Matrix4.CreateRotationZ(rotation.Z);
			model *= Matrix4.CreateScale(scale);
			
			return model;
		}
	}
}
