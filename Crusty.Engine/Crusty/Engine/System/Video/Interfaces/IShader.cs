using Crusty.Engine.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine
{
	public interface IShader : IDisposable
	{
		void Use();

		void Unuse();

		void Create(string fileName);

		void Compile(string filename);

		void Cleanup();

		void Set_Float(string name, float value);
		void Set_Vec2(string name, Vector2 value);
		void Set_Vec3(string name, Vector3 value);
		void Set_Vec4(string name, Vector4 value);

		void Set_Mat4(string name, Matrix4 value);

		void Set_Light(IList<Light> light);
		
		void Set_Int(string name, int value);

	}
}
