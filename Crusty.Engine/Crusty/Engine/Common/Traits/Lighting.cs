using System;
using Crusty.Engine.Common.Traits;
using OpenTK;

namespace Crusty.Engine.Traits
{
	public class Lightning : MoveAble
	{
		public Vector3 LightColor { get; internal set; }
		public Vector3 Attenuation { get; internal set; } = new Vector3(1, 0, 0);
	}
}
