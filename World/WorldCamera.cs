using Godot;

namespace Redwood.World;

public partial class WorldCamera : Node2D
{
	public override void _Process(double delta)
	{
		HandleZoom();
	}

	private static void HandleZoom()
	{
		var delta = new Vector2(0.1f, 0.1f);

		if (Input.IsActionJustPressed("zoom_out"))
		{
			var diff = PlayerManager.PlayerNode.PlayerCamera.Zoom - delta;
			if (diff < new Vector2(0f, 0f))
				return;

			PlayerManager.PlayerNode.PlayerCamera.Zoom = diff;
		}

		if (Input.IsActionJustPressed("zoom_in"))
		{
			var sum = PlayerManager.PlayerNode.PlayerCamera.Zoom - delta;
			// if (sum > new Vector2(1.4f, 1.4f))
			// 	return;

			PlayerManager.PlayerNode.PlayerCamera.Zoom = PlayerManager.PlayerNode.PlayerCamera.Zoom + delta;
		}
	}
}