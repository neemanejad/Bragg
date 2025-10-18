using System;

using Godot;

public partial class DamageIndicator : Node2D
{
	[Export]
	Sprite2D sprite { get; set; }
	Tween currentDamageAnimation { get; set; }
	Color originalColor { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalColor = sprite.Modulate;
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
		sprite.Modulate = Colors.Red;
		currentDamageAnimation.TweenProperty(sprite, Sprite2D.PropertyName.Modulate.ToString(), originalColor, 0.2);
	}
}