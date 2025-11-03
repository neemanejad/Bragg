using Godot;

using Redwood.Singletons;

namespace Redwood.Enemies;

public partial class ShootingEnemy : Node2D
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