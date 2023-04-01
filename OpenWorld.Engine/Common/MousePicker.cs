using System;
using OpenTK;
using OpenTK.Input;

namespace OpenWorld.Engine.Common
{

	public class MousePicker
	{
		Matrix4 ViewMatrix;
		Matrix4 ProjMatix;
		public Vector3 CurrentRay;

		int width;
		int height;

		Camera camera;

		public MousePicker(Camera camera)
		{
			this.camera = camera;
			ViewMatrix = camera.ViewMatrix;
			ProjMatix = camera.ProjectionMatrix;
		}

		public void Update(int width, int height)
		{
			this.width = width;
			this.height = height;

			CurrentRay = calculateMouseRay();

		}

		public Vector3 calculateMouseRay()
		{
			float mouseX = Mouse.GetState().X;
			float mouseY = Mouse.GetState().Y; ;

			var NomalizeddeviceCoords = GetNormalizedDeviceCoords(mouseX, mouseY);
			var ClipCoords = new Vector4(NomalizeddeviceCoords.X, NomalizeddeviceCoords.Y, -1, 1);
			var eyeCoords = ToEyeCoords(ClipCoords);

			return toWorldCoords(eyeCoords);

		}

		Vector2 GetNormalizedDeviceCoords(float mouseX, float mouseY)
		{
			float x = (2 * mouseX) / width - 1;
			float y = (2 * mouseY) / height - 1;

			return new Vector2(x, -y);
		}

		Vector4 ToEyeCoords(Vector4 clipCoords)
		{
			Matrix4 inverseProjMatrix = ProjMatix.Inverted();
			Vector4 eyeCoords = Vector4.Transform(inverseProjMatrix, clipCoords);

			return new Vector4(eyeCoords.X, eyeCoords.Y, -1, 0f);
		}

		Vector3 toWorldCoords(Vector4 eyeCoords)
		{
			var inversViewMatrix = ViewMatrix.Inverted();
			var worldCoords = Vector4.Transform(inversViewMatrix, eyeCoords);

			return new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z).Normalized();
		}

	}
}
