using Godot;

public partial class StartGameArea : Area2D
{
	StartGame startGameNode;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startGameNode = GetNode<StartGame>("StartGame");
		startGameNode.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEnteredStartGameArea(Node2D body)
	{
		startGameNode.Show();
	}

	public void OnBodyExitedStartGameArea(Node2D body)
	{
		startGameNode.Hide();
	}
}
