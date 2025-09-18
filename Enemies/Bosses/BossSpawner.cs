using Godot;
using System;

public partial class BossSpawner : Node2D
{
	[Signal]
	public delegate void BossSpawnedEventHandler();

	[Export]
	public PackedScene BossPackedScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SpawnBoss()
	{
        if (PlayerManager.PlayerNode == null)
        {
            return;
        }

        var enemyNode = BossPackedScene.Instantiate<Node2D>();

        enemyNode.Position = LocationUtils.Instance.CreateRandomPosition();

        AddChild(enemyNode);

        EmitSignal(SignalName.BossSpawned);
	}
}
