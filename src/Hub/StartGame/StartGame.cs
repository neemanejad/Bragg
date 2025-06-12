using Godot;
using System;

public partial class StartGame : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("StartGame menu");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBeginPressed()
	{
		GD.Print("Start game!");
	}
}
