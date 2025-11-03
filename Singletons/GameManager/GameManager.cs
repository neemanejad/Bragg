using System;

using Godot;

using Redwood.Singletons;
using Redwood.UI;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	UISignalEmitter _uiSignalEmitter { get; set; }

	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	[Signal]
	public delegate void LevelClearedEventHandler();

	Timer survivalTimer = new Timer();


	public int SurvivalTime { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		survivalTimer.WaitTime = 1;
		survivalTimer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.AddSecond));
		AddChild(survivalTimer);

		_uiSignalEmitter = ResourceLoader.Load<PackedScene>("res://UI/UISignalEmitter.tscn").Instantiate<UISignalEmitter>();
		AddChild(_uiSignalEmitter);
	}

	public void PauseSurvivalTimer()
	{
		survivalTimer.Stop();
	}

	public void ResumeSurvivalTimer()
	{
		survivalTimer.Start();
	}

	public void ResetSurvivalTimer()
	{
		survivalTimer.Stop();
		SurvivalTime = 0;
	}

	private void AddSecond()
	{
		SurvivalTime += 1;
	}

	public void Start()
	{
		EmitSignal(SignalName.StartGame);
	}

	public void End()
	{
		_uiSignalEmitter.SignalAddUI(UIType.GameOverMenu);
		PlayerManager.Instance.Reset();

		EmitSignal(SignalName.EndGame);
	}

	public void LevelClearedSignal()
	{
		EmitSignal(SignalName.LevelCleared);
	}
}