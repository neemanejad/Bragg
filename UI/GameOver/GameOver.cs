using System;

using Godot;

using Redwood.Singletons.SceneSwitcher;
using Redwood.Utils;


public partial class GameOver : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		var timeAliveLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/VBoxContainer/PanelContainer/MarginContainer/HBoxContainer/TimeAliveLabelValue");
		timeAliveLabel.Text = TimeUtils.FormatSecondsToSurvivalTime(GameManager.Instance.SurvivalTime);

		var enemiesKilledLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/PanelContainer/VBoxContainer/PanelContainer2/MarginContainer/HBoxContainer/KillsLabelValue");
		enemiesKilledLabel.Text = PlayerManager.Instance.KillCount.ToString();

		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnContinueButtonPressed()
	{
		SceneSwitcher.Instance.GotoScene("res://Hub/Hub.tscn");
		PlayerManager.Instance.Reset();
		GetTree().Paused = false;
		QueueFree();
	}
}