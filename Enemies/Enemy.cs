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

	private bool canAttack = true;

	public override void _Ready()
	{
		if (HealthComponent == null)
		{
			throw new NullReferenceException("HealthComponent is null in HitboxComponent");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		FollowPlayer(delta);
	}

	public void LostAllHealth()
	{
		PlayerManager.Instance.OnKillHandler();
		MaybeDropItem();
		QueueFree();
	}

	private void FollowPlayer(double delta)
	{
		if (PlayerManager.PlayerNode == null)
		{
			return;
		}

		var playerDirection =  (PlayerManager.PlayerNode.GlobalPosition - GlobalPosition).Normalized();
		GlobalPosition += playerDirection * (float)(Speed * delta);
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
