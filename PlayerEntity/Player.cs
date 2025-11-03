using Godot;

using Redwood.Items;
using Redwood.Singletons;

namespace Redwood.PlayerEntity;

public partial class Player : CharacterBody2D
{
	[Export]
	public double Speed { get; set; } = 400;

	public Camera2D PlayerCamera { get; private set; }

	public override void _Ready()
	{
		PlayerManager.Instance.SetPlayerNode(this);

		GetNode<HealthComponent>("Vitals/HealthComponent").SetHealth(PlayerManager.Instance.Health);
		PlayerCamera = GetNode<Camera2D>("PlayerCamera");

		ApplyExistingItemEffectsToPlayer();
	}

	public void HandleMovementInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * (float)Speed;

		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleMovementInput();
	}

	public override void _Process(double delta)
	{
		//if (Input.IsActionJustPressed("inventory"))
		//{
		//	_uiSignalEmitter.SignalAddUI(UIType.InventoryMenu);
		//}

		//if (Input.IsActionJustPressed("escape"))
		//{
		//	_uiSignalEmitter.SignalAddUI(UIType.PauseMenu);
		//}
	}

	/// <summary>
	/// Apply all of the existing item effects on player. This should only
	/// be called once in <see cref="_Ready"/>.
	/// </summary>
	private void ApplyExistingItemEffectsToPlayer()
	{
		foreach (var entry in PlayerManager.Instance.ItemCountMap)
		{
			for (var i = 0; i < entry.Value; i++)
			{
				Item.GetItem(entry.Key).Effect(this);
			}
		}
	}
}