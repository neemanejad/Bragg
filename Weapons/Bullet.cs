using Godot;

using Redwood.Components;

namespace Redwood.Weapons;

public partial class Bullet : Node2D
{
	[Export]
	public float Damage;

	[Export]
	public HurtBox BulletHurtBox;

	[Export]
	public VisibleOnScreenNotifier2D VisibleNotifier;

	public override void _Ready()
	{
		BulletHurtBox.Damage = Damage;
		BulletHurtBox.Connect(HurtBox.SignalName.Hit, new Callable(this, Node.MethodName.QueueFree));
		VisibleNotifier.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new Callable(this, Node.MethodName.QueueFree));
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.X * 100;
	}
}