using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine
{
	public class Input
	{
		public delegate void InputKeyDownEventHandler(object sender, InputKeyEventArgs e);
		public event InputKeyDownEventHandler InputKeyDown;

		public delegate void InputKeyUpEventHandler(object sender, InputKeyEventArgs e);
		public event InputKeyDownEventHandler InputKeyUp;

		public Dictionary<Key, KeyboardKeyState> KeyStates;
		public Dictionary<MouseButton, MouseButtonState> ButtonStates;

		public CursorPosition MousePosition {get; private set;}


		public Input()
		{
			KeyStates = new Dictionary<Key, KeyboardKeyState>();
			ButtonStates = new Dictionary<MouseButton, MouseButtonState>();
		}

		public void Initialize()
		{

		}

		public MouseButtonState GetState(MouseButton button) => ButtonStates[button];

		public KeyboardKeyState GetState(Key key) => KeyStates[key];

		public void SetState(MouseButton button, CursorPosition position, bool pressed)
		{
			if (!pressed && ButtonStates.ContainsKey(button))
				ButtonStates.Remove(button);

			ButtonStates.Add(button, new MouseButtonState(button, position, pressed));
		}

		public void Update(double deltatime)
		{
			if (ButtonStates.Count != 0)
			{
				var pressedButtons = ButtonStates.Values.Where(btn => btn.IsPressed).LastOrDefault();
				if (pressedButtons.IsPressed)
					Console.WriteLine(pressedButtons.Key);
			}

			if (KeyStates.Count != 0)
			{
				var pressedButtons = KeyStates.Values.Where(btn => btn.IsPressed).LastOrDefault();
					InputKeyDown?.DynamicInvoke(this, new InputKeyEventArgs(pressedButtons.Key, pressedButtons.IsPressed));
			}
		}

		public void SetMousePosition(CursorPosition position)
		{
			MousePosition = position;
		}

		public void SetState(Key key, bool pressed)
		{
			if (KeyStates.ContainsKey(key))
				KeyStates.Remove(key);

			KeyStates.Add(key, new KeyboardKeyState(key, pressed));
		}

		public void Start()
		{

		}

		public void Stop()
		{

		}


		public bool IsPressed(Key key) => KeyStates[key].IsPressed;

		public bool IsPressed(MouseButton button) => ButtonStates[button].IsPressed;
	}
}
