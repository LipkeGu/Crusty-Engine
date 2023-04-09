using OpenTK;

namespace Crusty.Engine.UI
{
	public class Control : IControl
	{
		public Control(string name, Vector2 position)
		{
			Position = position;
			Name = name;
		}

		public string Name { get; set; }

		public Vector2 Position { get; set; }

		public void Draw()
		{
		}
	}
}