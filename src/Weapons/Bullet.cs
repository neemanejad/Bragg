using Godot;

public partial class Bullet : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("bullet");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * 100;
	}

	public void Hit(Node2D body)
	{
		GD.Print("Hit");
		GD.Print(body);
	}

	public void FreeBullet()
	{
		GD.Print("Free bullet");
		QueueFree();
	}
}
