using Godot;

namespace Redwood.Enemies;

[GlobalClass]
public partial class EnemyData : Resource
{
	[Export]
	public string Id { get; set; }
	[Export]
	public string Name { get; set; }

	[Export]
	public double Speed = 200;

	[Export]
	public float Damage { get; set; }
}