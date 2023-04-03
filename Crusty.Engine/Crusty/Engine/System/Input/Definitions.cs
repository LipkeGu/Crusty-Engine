using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Crusty.Engine
{
	public struct KeyboardKeyState
	{
		public Key Key { get; set; }
		public bool IsPressed { get; set; }

		public bool ModifiedAlt { get; private set; }
		public bool ModifiedShift { get; private set; }

		public KeyboardKeyState(Key key, bool isPressed, bool shiftModified, bool altmodified)
		{
			IsPressed = isPressed;
			Key = key;
			ModifiedAlt = altmodified;
			ModifiedShift = shiftModified;
		}
	}

	public struct CursorPosition
	{
		public int X { get; private set; }

		public int Y { get; private set; }
		
		public CursorPosition(float x, float y)
		{
			X = (int)Math.Round(x,0);
			Y = (int)Math.Round(y, 0);
		}

		public CursorPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public struct MouseButtonState
	{
		public MouseButton Key;

		public bool IsPressed;

		/// <summary>
		/// The Position where the button was pressed.
		/// </summary>
		public CursorPosition Position;

		/// <summary>
		/// The Position where the button was pressed (Picking).
		/// </summary>
		public Vector3 RayPosition;

		public MouseButtonState(MouseButton key, CursorPosition position, Vector3 pickPosition, bool isPressed)
		{
			Position = position;
			IsPressed = isPressed;
			RayPosition = pickPosition;
			Key = key;
		}
	}
}
