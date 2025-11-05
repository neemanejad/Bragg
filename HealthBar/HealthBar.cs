using System;

using Godot;

using Redwood.Components;

public partial class HealthBar : Control
{
	[Signal]
	public delegate void LostAllHealthEventHandler();
	[Export]
	public HealthComponent HealthComponent { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (HealthComponent == null)
		{
			throw new NullReferenceException("Health component is null on on HealthBar");
		}

		var healthBar = GetNode<ProgressBar>("ProgressBar");
		healthBar.MaxValue = HealthComponent.MaxHealth;
		healthBar.Value = HealthComponent.CurrentHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetNode<ProgressBar>("ProgressBar").Value = HealthComponent.CurrentHealth;
	}
}