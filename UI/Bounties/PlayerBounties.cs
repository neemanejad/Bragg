using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using Redwood.Bounties;
using Redwood.UI;

public partial class PlayerBounties : UI
{
	VBoxContainer BountyList { get; set; }
	CanvasLayer CanvasLayer { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CanvasLayer = GetNode<CanvasLayer>("CanvasLayer");
		BountyList = GetNode<VBoxContainer>("CanvasLayer/PanelContainer/MarginContainer/ScrollContainer/PlayerBountyList");

		BountyManager.Instance.Connect(BountyManager.SignalName.OnPlayerBountyAdded, new Callable(this, MethodName.PlayerBountyAdded));
		BountyManager.Instance.Connect(BountyManager.SignalName.OnPlayerBountyRemoved, new Callable(this, MethodName.PlayerBountyRemoved));


		foreach (var bounty in BountyManager.Instance.PlayerBounties)
		{
			PlayerBounty playerBounty = ResourceLoader.Load<PackedScene>("res://UI/Bounties/PlayerBounty.tscn").Instantiate<PlayerBounty>();
			playerBounty.Bounty = bounty;
			BountyList.AddChild(playerBounty);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("bounties") || Input.IsActionJustPressed("escape"))
		{
			QueueFree();
		}
	}

	void PlayerBountyAdded(string bountyId)
	{
		PlayerBounty playerBounty = ResourceLoader.Load<PackedScene>("res://UI/Bounties/PlayerBounty.tscn").Instantiate<PlayerBounty>();
		playerBounty.Bounty = BountyManager.Instance.PlayerBounties.Find((bounty) => bounty.Id == bountyId);
		BountyList.AddChild(playerBounty);
	}

	void PlayerBountyRemoved(string bountyId)
	{
		List<Node> children = BountyList.GetChildren().ToList();
		foreach (var child in children)
		{
			if (child is PlayerBounty && ((PlayerBounty)child).Bounty.Id == bountyId)
			{
				child.QueueFree();
			}
		}

	}
}