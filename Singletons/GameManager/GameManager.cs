using System;
using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	Timer survivalTimer = new Timer();


	public int SurvivalTime { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		survivalTimer.WaitTime = 1;
		survivalTimer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.AddSecond));
		AddChild(survivalTimer);
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
		var playerNode = PlayerManager.PlayerNode;

		var gameOverScene = GD.Load<PackedScene>("res://UI/GameOver/GameOver.tscn").Instantiate();
		gameOverScene.ProcessMode = ProcessModeEnum.Always;

		playerNode.GetParent().AddChild(gameOverScene);
		playerNode.QueueFree();

		PlayerManager.ClearPlayerNode();
		PlayerManager.Instance.Reset();

		EmitSignal(SignalName.EndGame);
	}
}
