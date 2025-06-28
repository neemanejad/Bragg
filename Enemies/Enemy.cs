using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public HealthComponent HealthComponent { get; set; }

	[Export]
	public Timer HitTimer { get; set; }

	private bool canAttack = true;

	public override void _Ready()
	{
		if (HealthComponent == null)
		{
			throw new NullReferenceException("HealthComponent is null in HitboxComponent");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LostAllHealth()
	{
		PlayerManager.Instance.OnKillHandler();
		QueueFree();
	}
}
