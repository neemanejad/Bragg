using System;

using Godot;

public partial class ShootingEnemy : Enemy
{
	[Export]
	public PackedScene Projectile { get; set; }
	[Export]
	public float ProjectileSpeed { get; set; }
	[Export]
	public float ProjectileDamage { get; set; }

	public override void _Ready()
	{
		Connect(SignalName.ReceivedDamage, new Callable(GetNode<DamageIndicator>("DamageIndicator"), DamageIndicator.MethodName.ShowDamage));

		base._Ready();
	}

	public void ShootProjectileTowardsPlayer()
	{
		// TODO: remove need for this check
		if (PlayerManager.Instance.PlayerNode == null)
		{
			return;
		}

		var projectile = Projectile.Instantiate<EnemyBullet>();
		projectile.Direction = PlayerManager.Instance.PlayerNode.GlobalPosition - GlobalPosition;
		projectile.Speed = ProjectileSpeed;
		projectile.Damage = ProjectileDamage;
		AddChild(projectile);
	}
}