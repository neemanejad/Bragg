using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec)

	public Vector2 ScreenSize; // Size of the game window

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessMovement(delta);
		CameraStuff();

		// var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");


	}

	private void CameraStuff()
	{
	}

	private void ProcessMovement(double delta)
	{
		var velocity = Vector2.Zero; // Movement vector

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			// animatedSprite2D.Play();
		}
		else
		{
			// animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;

		if (velocity.X != 0)
		{
			// animatedSprite2D.Animation = "walk";
			// animatedSprite2D.FlipV = false;
			// animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			// animatedSprite2D.Animation = "up";
			// animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}
}
