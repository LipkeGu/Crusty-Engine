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
		=> new Vector3(float.Parse(x, CultureInfo.InvariantCulture.NumberFormat),
			float.Parse(y, CultureInfo.InvariantCulture.NumberFormat),
				float.Parse(z, CultureInfo.InvariantCulture.NumberFormat));

		public static Vector3 CalculateFront(float pitch, float yaw)
		{
			var _front = -Vector3.UnitZ;
			_front.X = (float)Math.Cos(yaw) * (float)Math.Cos(pitch);
			_front.Y = (float)Math.Sin(pitch);
			_front.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);
			_front = Vector3.Normalize(_front);

			return _front;
		}

		public static Vector2 CreateVec2(string x, string y)
			=> new Vector2(float.Parse(x, CultureInfo.InvariantCulture.NumberFormat),
				float.Parse(y, CultureInfo.InvariantCulture.NumberFormat));

		public static Matrix4 Update_ViewMatrix(Vector3 position, Vector3 rotation, Vector3 front, Vector3 up)
		{
			var matrix = Matrix4.LookAt(position, position + front, up);
			matrix *= Matrix4.CreateRotationX(rotation.X);
			matrix *= Matrix4.CreateRotationY(rotation.Y);
			matrix *= Matrix4.CreateRotationZ(rotation.Z);

			return matrix;
		}

		public static Vector3 Unproject(CursorPosition pos, int width, int height, Matrix4 proj, Matrix4 view)
		{
			var ndc = new Vector3((2 * pos.X) / (width - 1.0f), (1.0f - (2.0f * pos.Y)) / height, 1.0f);
			var invclCoords = proj.Inverted() * new Vector4(ndc.X, ndc.Y, -ndc.Z, ndc.Z);

			return ((view.Inverted() * new Vector4(invclCoords.X, invclCoords.Y, -1.0f, 0.0f)).Xyz).Normalized();
		}

		public static Matrix4 Update_ProjectionMatrix(int width, int height, float near, float far = 1000.0f, float fov = 67.0f)
			=> Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), width / height, near, far);

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
