using System;

using Godot;

using Redwood.UI;

public partial class BountySelectionScreen : UIControl
{
	VBoxContainer _bountyList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_bountyList = GetNode<VBoxContainer>("CanvasLayer/PanelContainer/VBoxContainer/MarginContainer/ScrollContainer/BountyList");

		var exitButton = GetNode<Button>("CanvasLayer/PanelContainer/VBoxContainer/HBoxContainer/MarginContainer/ExitButton");
		exitButton.Pressed += () => QueueFree();

		RenderBountyOptions();

		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape"))
		{
			QueueFree();
		}
	}

	public void RenderBountyOptions()
	{
		var bounties = BountyManager.Instance.CurrentlyOfferedBounties;

		foreach (var bounty in bounties)
		{
			BountySelectionOption bountyOption = ResourceLoader.Load<PackedScene>("res://UI/Bounties/BountySelectionOption.tscn").Instantiate<BountySelectionOption>();
			bountyOption.Bounty = bounty;

			_bountyList.AddChild(bountyOption);
		}
	}

	public override void _ExitTree()
	{
		GetTree().Paused = false;

		base._ExitTree();
	}
}