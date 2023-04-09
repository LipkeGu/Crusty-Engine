using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;

namespace Crusty.Engine
{
	public class Input
	{
		public delegate void InputKeyDownEventHandler(object sender, InputKeyEventArgs e);
		public event InputKeyDownEventHandler InputKeyDown;

		public delegate void InputKeyUpEventHandler(object sender, InputKeyEventArgs e);
		public event InputKeyDownEventHandler InputKeyUp;

		public delegate void InputMouseButtonDownEventHandler(object sender, InputButtonEventArgs e);
		public event InputMouseButtonDownEventHandler InputMouseButtonDown;

		public delegate void InputMouseButtonUpEventHandler(object sender, InputButtonEventArgs e);
		public event InputMouseButtonUpEventHandler InputMouseButtonUp;


		public Dictionary<Key, KeyboardKeyState> KeyStates;
		public Dictionary<MouseButton, MouseButtonState> ButtonStates;

		public CursorPosition MousePosition {get; private set;}
		public Vector3 RayPosition { get; private set; }

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

		public void SetState(MouseButton button, CursorPosition position, Vector3 pickPosition, bool pressed)
		{
			if (ButtonStates.ContainsKey(button))
				ButtonStates.Remove(button);

				ButtonStates.Add(button, new MouseButtonState(button, position, pickPosition, pressed));
		}

		public void Update(double deltatime)
		{
			if (ButtonStates.Count != 0)
			{
				var pressedButtons = ButtonStates.Values.Where(btn => btn.IsPressed).LastOrDefault();
				if (pressedButtons.IsPressed)
					InputMouseButtonDown?.DynamicInvoke(this, new InputButtonEventArgs(pressedButtons.Key,pressedButtons.Position, RayPosition, pressedButtons.IsPressed,deltatime));
			}

			if (KeyStates.Count != 0)
			{
				var pressedButtons = KeyStates.Values.Where(btn => btn.IsPressed).LastOrDefault();
					InputKeyDown?.DynamicInvoke(this, new InputKeyEventArgs(pressedButtons.Key, pressedButtons.IsPressed, 
						deltatime,pressedButtons.ModifiedAlt, pressedButtons.ModifiedShift));
			}
		}

		public void SetMousePosition(Vector3 rayPosition, CursorPosition position)
		{
			MousePosition = position;
			RayPosition = rayPosition;
		}

		public void SetState(Key key, bool pressed, bool altPRessed, bool shiftPRessed)
		{
			if (KeyStates.ContainsKey(key))
				KeyStates.Remove(key);

			KeyStates.Add(key, new KeyboardKeyState(key, pressed, shiftPRessed, altPRessed));
		}

		public void Start()
		{

		}

		public void Stop()
		{
			ButtonStates.Clear();
			KeyStates.Clear();
		}


		public bool IsPressed(Key key) => KeyStates[key].IsPressed;

		public bool IsPressed(MouseButton button) => ButtonStates[button].IsPressed;
	}
}
