using System;

using Godot;

using Redwood.Items;
using Redwood.Singletons;

public partial class SpeedBoostItem : Node2D, IItemNode
{
	public ItemId ItemId
	{
		get
		{
			return ItemId.SPEED_BOOST;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPickup(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			PlayerManager.Instance.AddItem((int)ItemId);
			QueueFree();
		}
	}
}