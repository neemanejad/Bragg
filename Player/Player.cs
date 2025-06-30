using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	public void HandleMovementInput(double deltaTime)
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		GlobalPosition += (inputDirection * (float)(Speed * deltaTime));
	}

	public override void _PhysicsProcess(double delta)
	{
	}

    public override void _Process(double delta)
    {
		HandleMovementInput(delta);

		if (Input.IsActionJustPressed("interact"))
		{
			var weapon = GetNode<BaseWeapon>("CyclicFollowerComponent/SimpleGun");
			weapon.Shoot();
		}
    }
}
