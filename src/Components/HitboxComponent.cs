using Godot;
using System;

public partial class HitboxComponent : Node
{
	[Export]
	public HealthComponent HealthComponent { get; set; }
	[Export]
	public Timer HitTimer { get; set; }
	private bool canAttack = true;

	// Called when the node enters the scene tree for the first time.
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

	public void OnBodyEnteredHitBox(Node2D area)
	{
		GD.Print("enter");
		HealthComponent.RemoveHealth(10);
	}

	public void SetCanAttack()
	{
		canAttack = true;
	}

}
