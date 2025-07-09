using System;
using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	private double StartTimeUnix;

	public int ElapsedTime { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void Start()
	{
		StartTimeUnix = Time.GetUnixTimeFromSystem();

		EmitSignal(SignalName.StartGame);
	}

	public void End()
	{
		var playerNode = PlayerManager.PlayerNode;

		ElapsedTime = (int)(Time.GetUnixTimeFromSystem() - StartTimeUnix);

		var gameOverScene = GD.Load<PackedScene>("res://UI/GameOver/GameOver.tscn").Instantiate();

		playerNode.GetParent().AddChild(gameOverScene);
		playerNode.QueueFree();

		PlayerManager.ClearPlayerNode();
		PlayerManager.Instance.Reset();

		EmitSignal(SignalName.EndGame);
	}
}
