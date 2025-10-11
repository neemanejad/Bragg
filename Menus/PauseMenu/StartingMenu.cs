using Godot;

using Redwood.Singletons.SceneSwitcher;

namespace Redwood.Menus;

public partial class StartingMenu : CanvasLayer
{
	[Signal]
	public delegate void ClosePauseMenuEventHandler();
	[Signal]
	public delegate void ShowSettingsMenuEventHandler();

	Button _resumeButton;
	Button _settingsButton;
	Button _quitToHubButton;
	Button _quitToDesktopButton;
	CanvasLayer _settingsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_resumeButton = GetNode<Button>("MarginContainer/VBoxContainer/ResumeButton");
		_settingsButton = GetNode<Button>("MarginContainer/VBoxContainer/SettingsButton");
		_quitToHubButton = GetNode<Button>("MarginContainer/VBoxContainer/QuitToHubButton");
		_quitToDesktopButton = GetNode<Button>("MarginContainer/VBoxContainer/QuitToDesktopButton");

		_resumeButton.Pressed += OnResumeButtonClicked;
		_settingsButton.Pressed += OnSettingsButtonClicked;
		_quitToHubButton.Pressed += OnQuitToHubButtonClicked;
		_quitToDesktopButton.Pressed += OnQuitToDesktopButtonClicked;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessInput();
	}

	private void ProcessInput()
	{

	}

	public override void _ExitTree()
	{
		_resumeButton.Pressed -= OnResumeButtonClicked;
		_settingsButton.Pressed -= OnSettingsButtonClicked;
		_quitToHubButton.Pressed -= OnQuitToHubButtonClicked;
		_quitToDesktopButton.Pressed -= OnQuitToDesktopButtonClicked;
		base._ExitTree();
	}

	public void OnResumeButtonClicked()
	{
		EmitSignal(SignalName.ClosePauseMenu);
	}

	public void OnSettingsButtonClicked()
	{
		EmitSignal(SignalName.ShowSettingsMenu);
	}

	public void OnQuitToHubButtonClicked()
	{
		SceneSwitcher.Instance.GotoScene("res://Hub/Hub.tscn");
		EmitSignal(SignalName.ClosePauseMenu);
	}

	public void OnQuitToDesktopButtonClicked()
	{
		GetTree().Quit();
	}
}