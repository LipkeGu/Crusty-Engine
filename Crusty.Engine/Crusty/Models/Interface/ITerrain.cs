using Crusty.Engine.Common;
using Crusty.Engine.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.Crusty.Models.Interface
{
	public interface ITerrain
	{
		int Width { get; set; }

		int Height { get; set; }

		float QueryHeightAt(int x, int z);

		void Update(double deltaTime);

		void CleanUp();

		void Draw(ref GameWorldTime worldTime, ref IList<Light> light, ref Fog fog,
			Matrix4 projMatrix, Matrix4 viewMatrix, bool staticObject = false);
	}
}
