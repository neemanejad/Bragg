using System;

using Godot;

public partial class SettingsMenu : CanvasLayer
{
	[Signal]
	public delegate void CloseSettingsMenuEventHandler();

	CheckBox _fullscreenCheckbox;
	Button _backButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		ProcessMode = ProcessModeEnum.Disabled;

		_fullscreenCheckbox = GetNode<CheckBox>("MarginContainer/VBoxContainer/SettingsContainer/MarginContainer/CheckBox");
		_backButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/MarginContainer/Button");

		_fullscreenCheckbox.Pressed += OnFullscreenCheckboxPressed;
		_fullscreenCheckbox.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;

		_backButton.Pressed += () => EmitSignal(SignalName.CloseSettingsMenu);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.IsActionJustPressed("escape"))
		{
			EmitSignal(SignalName.CloseSettingsMenu);
		}
	}

	public void OnFullscreenCheckboxPressed()
	{
		if (_fullscreenCheckbox.ButtonPressed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	public override void _ExitTree()
	{
		_fullscreenCheckbox.Pressed -= OnFullscreenCheckboxPressed;
		base._ExitTree();
	}
}