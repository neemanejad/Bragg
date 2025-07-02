using Godot;

public partial class StartGame : Area2D
{
	public override void _Process(double delta)
	{
		var playerInArea = GetOverlappingBodies().Contains(PlayerManager.PlayerNode);

		var isInteracting = Input.IsActionJustPressed("interact");

		if (playerInArea && isInteracting)
		{
			SceneSwitcher.Instance.GotoScene("res://Levels/Main/Main.tscn");
		}
	}
}
