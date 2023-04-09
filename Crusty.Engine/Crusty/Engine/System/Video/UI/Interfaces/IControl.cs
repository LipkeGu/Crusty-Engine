using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.UI
{
	public interface IControl
	{
		string Name { get; set; }

		Vector2 Position { get; set; }
		void Draw();
	}
}
