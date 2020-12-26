using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Engine.Common
{
	public static class Functions
	{
		public static string[] SplitString(this string str, string delimeter)
		{
			return str.Split(delimeter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
