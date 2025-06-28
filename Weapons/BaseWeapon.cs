using Godot;
using Godot.Collections;

public partial class BaseWeapon : Node2D
{
	[Export]
	public PackedScene Bullet;

	[Export]
	public FirePattern FirePattern;

	[Export]
	public int MagazineCapacity;

	private int _currentMagazine;

	public override void _Ready()
	{
	}

	public void Shoot()
	{
		var bullet = Bullet.Instantiate<Bullet>();
		GetTree().Root.AddChild(bullet);

		bullet.Transform = GetNode<Marker2D>("Marker2D").GlobalTransform;
	}
}
