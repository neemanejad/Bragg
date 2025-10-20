using System;

using Godot;

public partial class DamageIndicator : Node2D
{
	public Sprite2D Sprite2D { get; set; }
	Tween currentDamageAnimation { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowDamage()
	{
		if (currentDamageAnimation != null)
		{
			currentDamageAnimation.Stop();
		}

		currentDamageAnimation = CreateTween();
		Sprite2D.Modulate = Colors.Red;
		currentDamageAnimation.TweenProperty(Sprite2D, Sprite2D.PropertyName.Modulate.ToString(), new Color(1, 1, 1, 1), 0.2);
	}
}