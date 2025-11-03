using Godot;

using Redwood.Components;

namespace Redwood.Weapons;

public partial class Bullet : Area2D
{
	[Export]
	float Damage { get; set; }

	public override void _Ready()
	{
		Connect(Area2D.SignalName.AreaEntered, new Callable(this, MethodName.Hit));
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * 100;
	}

	public void Hit(Area2D area)
	{
		if (area is HitBox hitBox)
		{
			hitBox.ReceiveDamage(Damage);
			QueueFree();
		}
	}

	public void FreeBullet()
	{
		QueueFree();
	}
}