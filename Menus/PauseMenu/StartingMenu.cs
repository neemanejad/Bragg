using Godot;

using Redwood.Singletons.SceneSwitcher;

namespace Redwood.Menus;

public partial class StartingMenu : CanvasLayer
{
	Button _resumeButton;
	Button _settingsButton;
	Button _quitToHubButton;
	Button _quitToDesktopButton;
	CanvasLayer _settingsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();

		ProcessMode = ProcessModeEnum.Always;

		_resumeButton = GetNode<Button>("MarginContainer/VBoxContainer/ResumeButton");
		_settingsButton = GetNode<Button>("MarginContainer/VBoxContainer/SettingsButton");
		_quitToHubButton = GetNode<Button>("MarginContainer/VBoxContainer/QuitToHubButton");
		_quitToDesktopButton = GetNode<Button>("MarginContainer/VBoxContainer/QuitToDesktopButton");

		_resumeButton.Pressed += OnResumeButtonClicked;
		_settingsButton.Pressed += OnSettingsButtonClicked;
		_quitToHubButton.Pressed += OnQuitToHubButtonClicked;
		_quitToDesktopButton.Pressed += OnQuitToDesktopButtonClicked;

		_settingsMenu = GetParent().GetNode<CanvasLayer>("SettingsMenu");
		_settingsMenu.VisibilityChanged += OnSettingsMenuVisibilityChangeDeferred;
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
		_resumeButton.Pressed -= OnResumeButtonClicked;
		_settingsButton.Pressed -= OnSettingsButtonClicked;
		_quitToHubButton.Pressed -= OnQuitToHubButtonClicked;
		_quitToDesktopButton.Pressed -= OnQuitToDesktopButtonClicked;
		_settingsMenu.VisibilityChanged -= OnSettingsMenuVisibilityChangeDeferred;
		base._ExitTree();
	}

	public void OnResumeButtonClicked()
	{
		Hide();
		GetTree().Paused = false;
	}

	public void OnSettingsButtonClicked()
	{
		_settingsMenu.Show();
		// Prevent input from being handled here
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public void OnQuitToHubButtonClicked()
	{
		SceneSwitcher.Instance.GotoScene("res://Hub/Hub.tscn");
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