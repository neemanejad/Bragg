using Godot;
using System;

public partial class SurvivalTimer : Node2D
{
	private Label timerLabel;
	public int seconds = 0;
	public Timer Timer { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timerLabel = GetNode<Label>("CanvasLayer/TimerLabel");
		Timer = GetNode<Timer>("Timer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSecondTimeout()
	{
		this.seconds++;
		UpdateTimerLabel();
	}

	public void UpdateTimerLabel()
	{
		timerLabel.Text = GetSurvivalTime();
    }

	public String GetSurvivalTime()
	{
        int minutes = this.seconds / 60;
        int seconds = this.seconds % 60;

		return $"{minutes:D2}:{seconds:D2}";
    }
}
