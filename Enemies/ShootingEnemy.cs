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

	public void ShootProjectileTowardsPlayer()
	{
		// TODO: remove need for this check
		if (PlayerManager.PlayerNode == null)
		{
			return;
		}

		var projectile = Projectile.Instantiate<EnemyBullet>();
		projectile.Direction = PlayerManager.PlayerNode.GlobalPosition - GlobalPosition;
		projectile.Speed = ProjectileSpeed;
		projectile.Damage = ProjectileDamage;
		AddChild(projectile);
	}
}