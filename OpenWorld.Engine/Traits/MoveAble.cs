using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Engine.Traits
{
	public class MoveAble
	{
		/// <summary>
		/// Gibt die Position eines Elements zurück.
		/// </summary>
		protected Vector3 Position;
		protected Vector3 Rotation;
		protected Vector3 Scale;
		protected MoveAble()
		{

		}

		public void Set_RotationX(float rot)
		{
			Rotation.X += rot;
		}

		public void Set_RotationY(float rot)
		{
			Rotation.Y += rot;
		}

		public void Set_RotationZ(float rot)
		{
			Rotation.Z += rot;
		}

		public void Set_PositionX(float pos)
		{
			Position.X += pos;
		}

		public void Set_PositionY(float pos)
		{
			Position.Y += pos;
		}

		public void Set_PositionZ(float pos)
		{
			Position.Z += pos;
		}
	}
}
