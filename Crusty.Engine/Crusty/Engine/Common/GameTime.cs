using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusty.Engine.Common
{
	public class GameWorldTime
	{
		public DateTime dateTime;

		public Vector3 LightColor;
		public float AmbientStrength;

		public int Hour;
		public int Minute;


		public GameWorldTime()
		{
			dateTime = new DateTime();
			AmbientStrength = 1.0f;
			LightColor = new Vector3(0.9f, 0.9f, 0.9f);

			Update();
		}

		public void Update()
		{
			Hour = dateTime.Hour;
			Minute = dateTime.Minute;


			switch (Hour)
			{
				case 0:
					AmbientStrength = 0.1f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 1:
					AmbientStrength = 0.1f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 2:
					AmbientStrength = !IsSummerTime ? 0.2f : 0.2f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 3:
					AmbientStrength = !IsSummerTime ? 0.3f : 0.4f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 4:
					AmbientStrength = !IsSummerTime ? 0.4f : 0.6f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 5:
					AmbientStrength = !IsSummerTime ? 0.5f : 0.8f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 6:
					AmbientStrength = !IsSummerTime ? 0.6f : 0.9f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 7:
					AmbientStrength = !IsSummerTime ? 0.7f : 1.0f;
					break;
				case 8:
					AmbientStrength = !IsSummerTime ? 0.8f : 1.0f;
					break;
				case 9:
					AmbientStrength = !IsSummerTime ? 0.9f : 1.0f;
					break;
				case 10:
					AmbientStrength = 1.0f;
					break;
				case 11:
					AmbientStrength = 1.0f;
					break;
				case 12:
					AmbientStrength = 1.0f;
					break;
				case 13:
					AmbientStrength = 1.0f;
					break;
				case 14:
					AmbientStrength = 1.0f;
					break;
				case 15:
					AmbientStrength = !IsSummerTime ? 0.9f : 1.0f;
					break;
				case 16:
					AmbientStrength = !IsSummerTime ? 0.8f : 1.0f;
					break;
				case 17:
					AmbientStrength = !IsSummerTime ? 0.7f : 1.0f;
					break;
				case 18:
					AmbientStrength = !IsSummerTime ? 0.6f : 1.0f;
					break;
				case 19:
					AmbientStrength = !IsSummerTime ? 0.5f : 1.0f;
					break;
				case 20:
					AmbientStrength = !IsSummerTime ? 0.4f : 0.9f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 21:
					AmbientStrength = !IsSummerTime ? 0.3f : 0.8f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 22:
					AmbientStrength = !IsSummerTime ? 0.2f : 0.6f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				case 23:
					AmbientStrength = !IsSummerTime ? 0.1f : 0.4f;
					LightColor = new Vector3(0.790459f, 1.049128f, 1.505834f);
					break;
				default:
					AmbientStrength = 0.9f;
					break;
			}
		}

		private bool IsSummerTime => dateTime.IsDaylightSavingTime();
	}
}
