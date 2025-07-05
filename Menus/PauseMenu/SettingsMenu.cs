using Godot;
using System;

public partial class SettingsMenu : CanvasLayer
{
    CheckBox fullscreenCheckbox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        fullscreenCheckbox = GetNode<CheckBox>("CheckBox");

        Hide();

        fullscreenCheckbox.Pressed += OnFullscreenCheckboxPressed;
        fullscreenCheckbox.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;

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
        if (fullscreenCheckbox.ButtonPressed)
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
        fullscreenCheckbox.Pressed -= OnFullscreenCheckboxPressed;
        base._ExitTree();
    }

    public bool CanReceiveInput()
    {
        return Visible;
    }
}
