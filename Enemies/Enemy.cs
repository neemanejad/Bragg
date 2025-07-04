using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public HealthComponent HealthComponent { get; set; }

	[Export]
	public Timer HitTimer { get; set; }

	[Export]
	public double Speed = 200;

	[Export]
	public float Damage { get; set; }

	private bool canAttack = true;

	public override void _Ready()
	{
		if (HealthComponent == null)
		{
			throw new NullReferenceException("HealthComponent is null in HitboxComponent");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		FollowPlayer();
		DealDamage();
	}

	public void DealDamage()
	{
		var lastSlideCollision = GetLastSlideCollision();

		if (lastSlideCollision == null)
		{
			return;
		}

		var collider = lastSlideCollision.GetCollider();

		if (collider != PlayerManager.PlayerNode)
		{
			return;
		}

		PlayerManager.Instance.SubtractHealth(Damage);
	}

	public void LostAllHealth()
	{
		PlayerManager.Instance.OnKillHandler();
		MaybeDropItem();
		QueueFree();
	}

	private void FollowPlayer()
	{
		if (PlayerManager.PlayerNode == null)
		{
			return;
		}

		var playerDirection = (PlayerManager.PlayerNode.GlobalPosition - GlobalPosition).Normalized();
		Velocity = playerDirection * (float)Speed;
		MoveAndSlide();
	}

	private void MaybeDropItem()
	{
		var random = new Random();
		var randomNumber = random.Next(0, 100);
		if (randomNumber >= 1)
		{
			var item = ResourceLoader.Load<PackedScene>("res://Items/SpeedBoostItem.tscn").Instantiate<SpeedBoostItem>();
			item.GlobalPosition = GlobalPosition;
			GetParent().CallDeferred(MethodName.AddChild, item);
		}
	}
}
