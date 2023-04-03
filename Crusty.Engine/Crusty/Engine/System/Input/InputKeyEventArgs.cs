using OpenTK.Input;

namespace Crusty.Engine
{
	public class InputKeyEventArgs
	{
		public Key Key { get; private set; }
		public bool Pressed { get; private set; }
		public bool ShiftPressed { get; private set; }
		public bool AltPressed { get; private set; }

		public float DeltaTIme { get; private set; }

		public InputKeyEventArgs(Key key, bool isPressed, double deltatime, bool alt, bool shift)
		{
			ShiftPressed = shift;
			AltPressed = alt;

			DeltaTIme = (float)deltatime;
			Key = key;
			Pressed = isPressed;
		}
	}
}