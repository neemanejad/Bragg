using Godot;

public partial class StartGame : Area2D
{
	public override void _Ready()
	{
		GameManager.Instance.PauseSurvivalTimer();
		GameManager.Instance.ResetSurvivalTimer();
		PlayerManager.Instance.HideSurvivalTimer();
		PlayerManager.Instance.StopShooting();
	}

	public override void _Process(double delta)
	{
		var playerInArea = GetOverlappingBodies().Contains(PlayerManager.PlayerNode);

		var isInteracting = Input.IsActionJustPressed("interact");

		if (playerInArea && isInteracting)
		{
			GameManager.Instance.Start();
			SceneSwitcher.Instance.GotoScene("res://Levels/Main/Main.tscn");
		}
	}
}