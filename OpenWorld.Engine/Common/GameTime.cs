using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Engine.Common
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
					break;
				case 1:
					AmbientStrength = 0.1f;
					break;
				case 2:
					AmbientStrength = 0.2f;
					break;
				case 3:
					AmbientStrength = 0.3f;
					break;
				case 4:
					AmbientStrength = 0.4f;
					break;
				case 5:
					AmbientStrength = 0.5f;
					break;
				case 6:
					AmbientStrength = 0.6f;
					break;
				case 7:
					AmbientStrength = 0.7f;
					break;
				case 8:
					AmbientStrength = 0.8f;
					break;
				case 9:
					AmbientStrength = 0.9f;
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
					AmbientStrength = 0.9f;
					break;
				case 16:
					AmbientStrength = 0.8f;
					break;
				case 17:
					AmbientStrength = 0.7f;
					break;
				case 18:
					AmbientStrength = 0.6f;
					break;
				case 19:
					AmbientStrength = 0.5f;
					break;
				case 20:
					AmbientStrength = 0.4f;
					break;
				case 21:
					AmbientStrength = 0.3f;
					break;
				case 22:
					AmbientStrength = 0.2f;
					break;
				case 23:
					AmbientStrength = 0.1f;
					break;
				default:
					AmbientStrength = 1.0f;
					break;
			}
		}

		private bool IsSummerTime => dateTime.IsDaylightSavingTime();
	}
}
