using System;

using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }

	public override void _Ready()
	{
		GameManager.Instance.ResetSurvivalTimer();
		GameManager.Instance.ResumeSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
		PlayerManager.Instance.StartShooting();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}