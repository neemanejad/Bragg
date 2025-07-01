using Godot;
using Redwood.Items;
using System;

public partial class Player : Area2D
{
	[Export]
	public double Speed { get; set; } = 400;


	public override void _Ready()
	{
        PlayerManager.Instance.SetPlayerNode(this);

        ApplyExistingItemEffectsToPlayer();
	}

	public void HandleMovementInput(double deltaTime)
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		GlobalPosition += (inputDirection * (float)(Speed * deltaTime));
	}

	public override void _PhysicsProcess(double delta)
	{
	}

    public override void _Process(double delta)
    {
		HandleMovementInput(delta);

		if (Input.IsActionJustPressed("interact"))
		{
			var weapon = GetNode<BaseWeapon>("CyclicFollowerComponent/SimpleGun");
			weapon.Shoot();
		}
    }

	/// <summary>
	/// Apply all of the existing item effects on player. This should only
	/// be called once in <see cref="_Ready"/>.
	/// </summary>
    private void ApplyExistingItemEffectsToPlayer()
	{
		foreach ( var entry in PlayerManager.Instance.ItemCountMap)
		{
			for ( var i = 0; i < entry.Value; i++ )
			{
				Item.GetItem(entry.Key).Effect(this);
			}
		}
	}
}
