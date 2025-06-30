using Godot;

public partial class StartGame : Area2D
{
	public override void _Process(double delta)
	{
		var playerInArea = GetOverlappingAreas().Contains(Owner.GetNode<Player>("Player"));

		var isInteracting = Input.IsActionJustPressed("interact");

		if (playerInArea && isInteracting)
		{
			SceneSwitcher.Instance.GotoScene("res://Levels/Main/Main.tscn");
		}
	}
}
