namespace Redwood.Components;

using Godot;

public partial class HitBox : Area2D
{
	[Export]
	public HealthComponent HealthComponent { get; set; }

	public void ReceiveDamage(float damage)
	{
		HealthComponent?.RemoveHealth(damage);
	}
}