using Godot;

public partial class Bullet : Area2D
{
	[Export]
	float Damage { get; set; }

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * 100;
	}

	public void Hit(Node2D body)
	{

		if (body is Enemy enemy)
		{
			enemy.TakeDamage(Damage);
			QueueFree();
		}
	}

	public void FreeBullet()
	{
		QueueFree();
	}
}