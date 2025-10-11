using System;

using Godot;

using Redwood;
using Redwood.UI;
using Redwood.UI.Deployment;

public partial class BountyComputer : Node2D
{
	[Export]
	UISignalEmitter _uISignalEmitter { get; set; }
	Label interactionLabel;
	PackedScene deploymentSelection;
	bool canInteract = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactionLabel = GetNode<Label>("InteractionLabel");
		deploymentSelection = ResourceLoader.Load<PackedScene>("res://UI/Bounties/BountySelectionScreen.tscn");
		interactionLabel.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (canInteract && Input.IsActionJustPressed("interact"))
		{
			_uISignalEmitter.SignalAddUI(UIType.BountySelectionMenu);
		}
	}

	public void OnPlayerEnterInteractionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			interactionLabel.Visible = true;
			canInteract = true;
		}
	}

	public void OnPlayerLeaveInteractionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			interactionLabel.Visible = false;
			canInteract = false;
		}
	}

}