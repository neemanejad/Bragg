using System;

using Godot;

using Project.Utils;

public partial class SurvivalTimer : Control
{
	private Label timerLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timerLabel = GetNode<Label>("CanvasLayer/TimerLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateTimerLabel();
	}

	public void UpdateTimerLabel()
	{
		timerLabel.Text = TimeUtils.FormatSecondsToSurvivalTime(GameManager.Instance.SurvivalTime);
	}
}