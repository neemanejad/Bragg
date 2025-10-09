using Godot;

using Redwood.Scripts.World;

using Redwood.Singletons.WorldManager;

namespace Redwood.World;

public partial class World : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }

	private WorldData _worldData;

	public override void _Ready()
	{
		GameManager.Instance.ResetSurvivalTimer();
		GameManager.Instance.ResumeSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
		PlayerManager.Instance.StartShooting();

		_worldData = WorldManager.Instance.SelectedWorld;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
			// if (diff < new Vector2(0.4f, 0.4f))
			// 	return;

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