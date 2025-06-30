using Godot;
using System;
using Project.Items;

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

	public void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("Player"))
		{
			PlayerManager.Instance.AddItem((int)ItemId);
			QueueFree();
		}
	}
}
