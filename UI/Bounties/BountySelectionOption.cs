using System;

using Godot;

using Redwood.Bounties;

public partial class BountySelectionOption : Button
{
	public Bounty Bounty { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BountyManager.Instance.Connect(BountyManager.SignalName.OnPlayerBountyAdded, new Callable(this, MethodName.OnBountyManagerBountyAdded));

		var bountyNameLabel = GetNode<Label>("BountyName");
		bountyNameLabel.Text = Bounty.Name;

		var bountyDescriptionLabel = GetNode<Label>("BountyDescription");
		bountyDescriptionLabel.Text = Bounty.Description;

		var reputationRewardLable = GetNode<Label>("VBoxContainer/ReputationReward");
		if (Bounty.ReputationReward != null)
		{
			reputationRewardLable.Text = Bounty.ReputationReward.ToString();
		}
		else
		{
			reputationRewardLable.Text = "";
		}

		var currencyRewardLable = GetNode<Label>("VBoxContainer/CurrencyReward");
		if (Bounty.CurrencyReward != null)
		{
			currencyRewardLable.Text = Bounty.CurrencyReward.ToString();
		}
		else
		{
			currencyRewardLable.Text = "";
		}

		var resourceRewardLable = GetNode<Label>("VBoxContainer/ResourceReward");
		if (Bounty.ResourceReward != null)
		{
			resourceRewardLable.Text = $"{Bounty.ResourceReward.Quantity}x {Bounty.ResourceReward.Resource.Name}";
		}
		else
		{
			resourceRewardLable.Text = "";
		}

		Pressed += () =>
		{
			BountyManager.Instance.AddPlayerBounty(Bounty);
		};

		CheckIfBountyShouldBeDisabled();
	}

	public void OnBountyManagerBountyAdded(string bountyId)
	{
		if (bountyId == Bounty.Id)
		{
			QueueFree();
		}

		CheckIfBountyShouldBeDisabled();
	}

	public void CheckIfBountyShouldBeDisabled()
	{
		// Determine if bounty should be disabled if max player bounties has been reached
		if (BountyManager.Instance.PlayerBounties.Count >= BountyManager.Instance.MaxBountyCount)
		{
			Disabled = true;
		}
		else
		{
			Disabled = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}