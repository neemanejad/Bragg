using System;
using System.Collections.Generic;

using Godot;

using Redwood.Enemies;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public EnemyData EnemyData { get; set; }
	[Export]
	public HealthComponent HealthComponent { get; set; }
	[Export]
	public Sprite2D Sprite2D { get; set; }
	[Signal]
	public delegate void EnemyDefeatedEventHandler(string? enemyId);
	[Signal]
	public delegate void ReceivedDamageEventHandler();

	private bool canAttack = true;

	public override void _Ready()
	{
		if (HealthComponent == null)
		{
			throw new NullReferenceException("HealthComponent is null in HitboxComponent");
		}

		var damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
		damageIndicator.Sprite2D = Sprite2D;

		Connect(SignalName.EnemyDefeated, new Callable(WorldEventListener.Instance, WorldEventListener.MethodName.OnEnemyKilled));
		Connect(SignalName.ReceivedDamage, new Callable(damageIndicator, DamageIndicator.MethodName.ShowDamage));
	}

	public override void _PhysicsProcess(double delta)
	{
		// TODO: remove need for this
		if (PlayerManager.Instance.PlayerNode == null)
		{
			return;
		}

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

		if (collider != PlayerManager.Instance.PlayerNode)
		{
			return;
		}

		PlayerManager.Instance.SubtractHealth(EnemyData.Damage);
	}

	public void TakeDamage(float damage)
	{
		HealthComponent.RemoveHealth(damage);
		EmitSignal(SignalName.ReceivedDamage);
	}

	public void LostAllHealth()
	{
		PlayerManager.Instance.OnKillHandler();
		MaybeDropItem();
		QueueFree();

		EmitSignal(SignalName.EnemyDefeated, EnemyData.Id);
	}

	private void FollowPlayer()
	{
		var playerDirection = (PlayerManager.Instance.PlayerNode.GlobalPosition - GlobalPosition).Normalized();
		Velocity = playerDirection * (float)EnemyData.Speed;
		MoveAndSlide();
	}

	private void MaybeDropItem()
	{
		var random = new Random();
		var randomNumber = random.Next(0, 100);
		if (randomNumber >= 90)
		{
			var item = ResourceLoader.Load<PackedScene>("res://Items/SpeedBoostItem.tscn").Instantiate<SpeedBoostItem>();
			item.GlobalPosition = GlobalPosition;
			GetParent().CallDeferred(MethodName.AddChild, item);
		}
	}
}