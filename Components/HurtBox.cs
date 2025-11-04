using Godot;

namespace Redwood.Components;

public partial class HurtBox : Area2D
{
	[Export]
	public float Damage;

	[Signal]
	public delegate void HitEventHandler();

	public override void _Ready()
	{
		Connect(Area2D.SignalName.AreaEntered, new Callable(this, MethodName.Hurt));
	}

	public void Hurt(Area2D area)
	{
		if (area is HitBox hitBox)
		{
			hitBox.ReceiveDamage(Damage);
			EmitSignal(SignalName.Hit);
		}
	}

	public override void _Process(double delta)
	{

	}
}