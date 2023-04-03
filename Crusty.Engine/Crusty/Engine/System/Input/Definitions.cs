using OpenTK.Input;
using System;
using System.Drawing;

namespace Crusty.Engine
{
	public struct KeyboardKeyState
	{
		public Key Key { get; set; }
		public bool IsPressed { get; set; }

		public KeyboardKeyState(Key key, bool isPressed)
		{
			IsPressed = isPressed;
			Key = key;
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
		public MouseButton Key { get; set; }

		public bool IsPressed { get; set; }

		/// <summary>
		/// The Position where the button was pressed.
		/// </summary>
		public CursorPosition Position { get; set; }

		public MouseButtonState(MouseButton key, CursorPosition position, bool isPressed)
		{
			Position = position;
			IsPressed = isPressed;
			Key = key;
		}
	}
}
