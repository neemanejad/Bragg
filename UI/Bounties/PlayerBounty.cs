using System;

using Godot;

using Redwood.Bounties;

public partial class PlayerBounty : Control
{
	public Bounty Bounty { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BountyManager.Instance.Connect(BountyManager.SignalName.OnPlayerBountyProgressUpdated, new Callable(this, MethodName.OnPlayerBountyUpdated));
		BountyManager.Instance.Connect(BountyManager.SignalName.OnPlayerBountyRemoved, new Callable(this, MethodName.OnPlayerBountyRemoved));

		var bountyNameLabel = GetNode<Label>("Panel/BountyName");
		bountyNameLabel.Text = Bounty.Name;

		var bountyDescriptionLabel = GetNode<Label>("Panel/BountyDescription");
		bountyDescriptionLabel.Text = Bounty.Description;

		var reputationRewardLabel = GetNode<Label>("Panel/VBoxContainer/ReputationReward");
		reputationRewardLabel.Text = Bounty.ReputationReward != null ? Bounty.ReputationReward.ToString() : "";

		var currentRewardLabel = GetNode<Label>("Panel/VBoxContainer/CurrencyReward");
		currentRewardLabel.Text = Bounty.CurrencyReward != null ? Bounty.CurrencyReward.ToString() : "";
		var resourceRewardlabel = GetNode<Label>("Panel/VBoxContainer/ResourceReward");
		resourceRewardlabel.Text = Bounty.ResourceReward != null ? Bounty.ResourceReward.ToString() : "";

		var currentProgressLabel = GetNode<Label>("Panel/HBoxContainer/CurrentProgress");
		currentProgressLabel.Text = Bounty.CurrentProgress.ToString();
		var maxProgressLabel = GetNode<Label>("Panel/HBoxContainer/MaxProgress");
		maxProgressLabel.Text = Bounty.MaxProgress.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void OnPlayerBountyUpdated(string bountyId)
	{
		if (bountyId != Bounty.Id)
		{
			return;
		}

		var currentProgressLabel = GetNode<Label>("Panel/HBoxContainer/CurrentProgress");
		currentProgressLabel.Text = Bounty.CurrentProgress.ToString();
		var maxProgressLabel = GetNode<Label>("Panel/HBoxContainer/MaxProgress");
		maxProgressLabel.Text = Bounty.MaxProgress.ToString();
	}

	public void OnPlayerBountyRemoved(string bountyId)
	{
		if (bountyId != Bounty.Id)
		{
			return;
		}

		QueueFree();
	}
}