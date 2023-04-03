using OpenTK;
using OpenTK.Input;

namespace Crusty.Engine
{
	public class InputButtonEventArgs
	{
		public MouseButton Button { get; private set; }
		public bool Pressed { get; private set; }

		public CursorPosition Position { get; private set; }
		public Vector3 RayPosition { get; private set; }

		public float DeltaTIme { get; private set; }

		public InputButtonEventArgs(MouseButton button, CursorPosition position, Vector3 CursorRay, bool isPressed, double deltatime)
		{
			DeltaTIme = (float)deltatime;
			Pressed = isPressed;
			RayPosition = CursorRay;
			Position = position;
			Button = button;
		}
	}
}