using Godot;
using System;

public partial class StartingMenu : CanvasLayer
{
    Button ResumeButton;
	Button SettingsButton;
	Button QuitToHubButton;
	Button QuitToDesktopButton;
	CanvasLayer SettingsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Hide();

        ProcessMode = ProcessModeEnum.Always;
        
		ResumeButton = GetNode<Button>("ResumeButton");
		SettingsButton = GetNode<Button>("SettingsButton");
		QuitToHubButton = GetNode<Button>("QuitToHubButton");
		QuitToDesktopButton = GetNode<Button>("QuitToDesktopButton");

		ResumeButton.Pressed += OnResumeButtonClicked;
		SettingsButton.Pressed += OnSettingsButtonClicked;
        QuitToHubButton.Pressed += OnQuitToHubButtonClicked;
        QuitToDesktopButton.Pressed += OnQuitToDesktopButtonClicked;

        SettingsMenu = GetParent().GetNode<CanvasLayer>("SettingsMenu");
        SettingsMenu.VisibilityChanged += OnSettingsMenuVisibilityChangeDeferred;
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
            Visible = !Visible;
            GetTree().Paused = Visible;
        }
    }

    public override void _ExitTree()
    {
        ResumeButton.Pressed -= OnResumeButtonClicked;
        SettingsButton.Pressed -= OnSettingsButtonClicked;
        QuitToHubButton.Pressed -= OnQuitToHubButtonClicked;
        QuitToDesktopButton.Pressed -= OnQuitToDesktopButtonClicked;
        SettingsMenu.VisibilityChanged -= OnSettingsMenuVisibilityChangeDeferred;
        base._ExitTree();
    }

	public void OnResumeButtonClicked()
	{
        Hide();
        GetTree().Paused = false;
	}

	public void OnSettingsButtonClicked()
	{
        SettingsMenu.Show();
        // Prevent input from being handled here
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public void OnQuitToHubButtonClicked()
	{
        SceneSwitcher.Instance.GotoScene("res://Levels/Hub/Hub.tscn");
        GetTree().Paused = false;
	}

	public void OnQuitToDesktopButtonClicked()
	{
        GetTree().Quit();
	}

	private void OnSettingsMenuVisibilityChangeDeferred()
	{
        // Deferring this call so input isn't handled here changing
        // the process mode to always
        CallDeferred(MethodName.OnSettingsMenuVisibilityChange);
	}

    public void OnSettingsMenuVisibilityChange()
    {
        var settingsMenu = GetParent().GetNode<CanvasLayer>("SettingsMenu");
        ProcessMode = settingsMenu.Visible ? ProcessModeEnum.Disabled : ProcessModeEnum.Always;
        Visible = !settingsMenu.Visible;
    }
}
