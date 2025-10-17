using Godot;

using Redwood.UI;

namespace Redwood.Bounties;

public partial class BountyComputer : Node2D
{
	[Export]
	UISignalEmitter UISignalEmitter { get; set; }

	[Export]
	public Label InteractionLabel;

	[Export]
	public PackedScene BountySelectionScreen;

	bool _canInteract = false;

	public override void _Ready()
	{
		InteractionLabel.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (_canInteract && Input.IsActionJustPressed("interact"))
		{
			UISignalEmitter.SignalAddUI(UIType.BountySelectionMenu);
		}
	}

	public void OnPlayerEnterInteractionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InteractionLabel.Visible = true;
			_canInteract = true;
		}
	}

	public void OnPlayerLeaveInteractionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InteractionLabel.Visible = false;
			_canInteract = false;
		}
	}

}