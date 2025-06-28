using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;

	[Export]
	public Inventory PlayerInventory;

	public Vector2 ScreenSize;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("interact"))
		{
			
			var weapon = GetNode<BaseWeapon>("CyclicFollowerComponent/SimpleGun");
			weapon.Shoot();
		}
    }
}
