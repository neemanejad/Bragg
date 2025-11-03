using System;

using Godot;

using Redwood.Singletons;

public partial class EnemyBullet : Node2D
{
	public Vector2 Direction { get; set; }
	public float Speed { get; set; }
	public float Damage { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition += Direction.Normalized() * Speed * (float)delta;
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			PlayerManager.Instance.SubtractHealth(Damage);
		}
	}


}