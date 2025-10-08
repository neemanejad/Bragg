using System;

using Godot;

public partial class WorldEventListener : Node
{
	public static WorldEventListener Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnResourceCollected(string resourceId, int quantity)
	{
		BountyManager.Instance.ResourceCollected(resourceId, quantity);
	}

	public void OnEnemyKilled(string enemyId)
	{
		BountyManager.Instance.EnemyKilled(enemyId);
	}
}