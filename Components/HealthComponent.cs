using System;

using Godot;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void LostAllHealthEventHandler();
	[Export]
	public float CurrentHealth { get; set; } = 100f;
	[Export]
	public float MaxHealth { get; set; } = 100f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void SetHealth(float healthPoints)
	{
		CurrentHealth = healthPoints;
	}

	public void AddHealth(float healthPoints)
	{
		CurrentHealth += healthPoints;
	}

	public void RemoveHealth(float healthPoints)
	{
		CurrentHealth -= healthPoints;

		if (CurrentHealth <= 0)
		{
			EmitSignal(SignalName.LostAllHealth);
		}
	}
}