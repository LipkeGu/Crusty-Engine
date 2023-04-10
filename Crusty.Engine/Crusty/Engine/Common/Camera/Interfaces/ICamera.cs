using Crusty.Engine.Common.Traits;
using Crusty.Engine.Crusty.Models.Interface;
using Crusty.Engine.Models;
using OpenTK;

namespace Crusty.Engine.Common.Camera
{
	public interface ICamera
	{
		Matrix4 ProjectionMatrix { get; set; }

		Matrix4 ViewMatrix { get; set; }

		Vector3 RayPosition { get; set; }

		Vector3 GetPosition();

		int Width { get; set; }

		int Height { get; set; }

		float Far { get; set; }

		float Near { get; set; }

		void Update(ITerrain terrain, double deltatime);

		void OnResize(int width, int height, float far);
		
		void OnMouseMove(CursorPosition cursorPosition);

		void Move_Forward(float speed);

		void Move_Backward(float speed);

		void Move_Left(float speed);

		void Move_Right(float speed);
	}
}
