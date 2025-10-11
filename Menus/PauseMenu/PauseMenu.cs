using System;

using Godot;

using Redwood.Menus;
using Redwood.UI;

public partial class PauseMenu : UI
{
	StartingMenu _startingMenu { get; set; }
	SettingsMenu _settingsMenu { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startingMenu = GetNode<StartingMenu>("StartingMenu");
		_settingsMenu = GetNode<SettingsMenu>("SettingsMenu");

		_startingMenu.Connect(StartingMenu.SignalName.ClosePauseMenu, new Callable(this, MethodName.Close));
		_startingMenu.Connect(StartingMenu.SignalName.ShowSettingsMenu, new Callable(this, MethodName.ShowSettingsMenu));

		_settingsMenu.Connect(SettingsMenu.SignalName.CloseSettingsMenu, new Callable(this, MethodName.CloseSettingsMenu));

		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape") && _startingMenu.Visible)
		{
			Close();
		}
	}

	public void Close()
	{
		QueueFree();
	}

	public void ShowSettingsMenu()
	{
		_startingMenu.Hide();
		_startingMenu.ProcessMode = ProcessModeEnum.Disabled;
		_settingsMenu.Show();
		_settingsMenu.ProcessMode = ProcessModeEnum.Always;
	}

	public void CloseSettingsMenu()
	{
		_settingsMenu.Hide();
		_settingsMenu.ProcessMode = ProcessModeEnum.Disabled;
		_startingMenu.Show();
		_startingMenu.ProcessMode = ProcessModeEnum.Always;
	}
}