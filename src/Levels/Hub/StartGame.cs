using Godot;

public partial class StartGame : Area2D
{
	public override void _Process(double delta)
	{
		var isInteracting = Input.IsActionJustPressed("interact");

		if (isInteracting)
		{
			SceneSwitcher.Instance.GotoScene("res://src/Levels/Main/Main.tscn");
		}
	}
}
