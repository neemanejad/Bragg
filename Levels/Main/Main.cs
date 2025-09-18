using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }

	public override void _Ready()
	{
        PlayerManager.Instance.ResetSurvivalTimer();
		PlayerManager.Instance.UnPauseSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
