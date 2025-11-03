using Godot;

using Redwood.Singletons;

namespace Redwood.Weapons;

public partial class BaseWeapon : Node2D
{
	[Export]
	public PackedScene Bullet;

	[Export]
	public FirePattern FirePattern;

	[Export]
	public int MagazineCapacity;

	private int _currentMagazine;
	Timer shootTimer;

	public override void _Ready()
	{
		shootTimer = GetNode<Timer>("ShootTimer");
		shootTimer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.Shoot));

		PlayerManager.Instance.SetWeapon(this);
	}

	public void StopShooting()
	{
		shootTimer.Stop();
	}

	public void StartShooting()
	{
		shootTimer.Start();
	}

	public void Shoot()
	{
		var bullet = Bullet.Instantiate<Bullet>();
		GetTree().Root.AddChild(bullet);

		bullet.Transform = GetNode<Marker2D>("Marker2D").GlobalTransform;
	}
}