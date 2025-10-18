using System;

using Godot;

public partial class StrongEnemy : Enemy
{
	public override void _Ready()
	{
		Connect(SignalName.ReceivedDamage, new Callable(GetNode<DamageIndicator>("DamageIndicator"), DamageIndicator.MethodName.ShowDamage));

		base._Ready();
	}
}