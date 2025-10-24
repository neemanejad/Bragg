using System;

using Godot;

namespace Redwood.World.WorldOverlay;

public partial class WorldOverlay : CanvasLayer
{
	[Export]
	public Timer Countdown;

	[Export]
	public Label CountdownLabel;

	public override void _Process(double delta)
	{
		double minutes = Countdown.TimeLeft / 60;
		double seconds = Countdown.TimeLeft % 60;

		string timeLeft = Math.Floor(minutes) + ":" + Math.Floor(seconds);

		CountdownLabel.Text = timeLeft;
	}

}