
using System;
using System.Collections.Generic;

using Godot;

public partial class FastEnemy : Enemy
{
	public override void _Ready()
	{
		Connect(SignalName.ReceivedDamage, new Callable(GetNode<DamageIndicator>("DamageIndicator"), DamageIndicator.MethodName.ShowDamage));

		base._Ready();
	}
}