using OpenTK.Input;

namespace Crusty.Engine
{
	public class InputKeyEventArgs
	{
		public Key Key { get; private set; }
		public bool Pressed { get; private set; }

		public InputKeyEventArgs(Key key, bool isPressed)
		{
			Key = key;
			Pressed = isPressed;
		}
	}
}