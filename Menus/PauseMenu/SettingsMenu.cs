using System;

using Godot;

public partial class SettingsMenu : CanvasLayer
{
	CheckBox _fullscreenCheckbox;
	Button _backButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fullscreenCheckbox = GetNode<CheckBox>("MarginContainer/VBoxContainer/SettingsContainer/MarginContainer/CheckBox");
		_backButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/MarginContainer/Button");

		Hide();

		_fullscreenCheckbox.Pressed += OnFullscreenCheckboxPressed;
		_fullscreenCheckbox.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;

		_backButton.Pressed += () => Visible = !Visible;

		ProcessMode = ProcessModeEnum.Always;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (!CanReceiveInput())
		{
			return;
		}

		if (Input.IsActionJustPressed("escape"))
		{
			Visible = !Visible;
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

	public bool CanReceiveInput()
	{
		return Visible;
	}
}