using Godot;
using System;

public partial class ExtractionPoint : Node2D
{
	SceneSwitcher sceneSwitcher;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneSwitcher = GetNode<SceneSwitcher>("SceneSwitcher");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEnteredExtractionPoint(Node2D body)
	{
		// TODO: Change this to something useful
		sceneSwitcher.SwitchScene();
	}
}
