using System;
using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;

using Redwood.Bounties;
using Redwood.Enemies;
using Redwood.Notifications;
using Redwood.Resources;
using Redwood.Singletons.NotificationsManager;

public partial class BountyManager : Node
{
	RandomNumberGenerator _rng = new RandomNumberGenerator();
	public static BountyManager Instance { get; private set; }
	public int MaxBountyCount = 5;
	//public System.Collections.Generic.Dictionary<string, Bounty> PlayerBountyMap { get; private set; } = new System.Collections.Generic.Dictionary<string, Bounty>() { };
	public List<Bounty> PlayerBounties = new() { };
	public List<Bounty> CurrentlyOfferedBounties = new() { };

	[Signal]
	public delegate void OnPlayerBountyAddedEventHandler(string bountyId);
	[Signal]
	public delegate void OnPlayerBountyRemovedEventHandler(string bountyId);
	[Signal]
	public delegate void OnPlayerBountyProgressUpdatedEventHandler(string bountyId);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void AddPlayerBounty(Bounty bounty)
	{
		if (PlayerBounties.Count >= MaxBountyCount)
		{
			return;
		}

		PlayerBounties.Add(bounty);
		CurrentlyOfferedBounties.Remove(bounty);

		EmitSignal(SignalName.OnPlayerBountyAdded, bounty.Id);
	}

	public void RemovePlayerBounty(Bounty bounty)
	{
		var index = PlayerBounties.FindIndex((playerBounty) => playerBounty.Id == bounty.Id);
		PlayerBounties.RemoveAt(index);

		EmitSignal(SignalName.OnPlayerBountyRemoved, bounty.Id);
	}

	public void GenerateBounties(int numOfbounties)
	{
		CurrentlyOfferedBounties.Clear();

		for (int i = 0; i < 10; i++)
		{
			CurrentlyOfferedBounties.Add(GenerateBounty());
		}
	}

	public Bounty GenerateBounty()
	{
		var enemyResourceDir = DirAccess.Open("res://Enemies/Resources");
		var enemyResourceFileNames = new Array<string>(enemyResourceDir.GetFiles()).ToList().Select((file) => file = $"{enemyResourceDir.GetCurrentDir()}/{file}");
		var randomEnemyResourceFileName = new Array<string>(enemyResourceFileNames).PickRandom();
		var randomEnemyResource = ResourceLoader.Load<EnemyData>(randomEnemyResourceFileName);
		var randomEliminationCount = _rng.RandiRange(3, 10);

		Bounty bounty = new EliminationBounty($"Eliminate {randomEliminationCount}x {randomEnemyResource.Name}", "This is a test elimination bounty, expect things to not work", randomEnemyResource.Id, 5, 100, false, null, null, null, randomEliminationCount);

		return bounty;
	}

	public void ResourceCollected(string resourceId, int quantity)
	{
		ProgressResourceBounties(resourceId, quantity);
	}

	public void EnemyKilled(string enemyId)
	{
		ProgressEliminationBounties(enemyId);
	}

	void ProgressResourceBounties(string resourceId, int quantity)
	{
		throw new NotImplementedException();
	}

	void ProgressEliminationBounties(string enemyId)
	{
		List<Bounty> finishedBounties = new List<Bounty>();
		List<Bounty> eliminationBounties = PlayerBounties.Where((bounty) => bounty is EliminationBounty).ToList();

		foreach (var bounty in PlayerBounties)
		{
			if (bounty.TargetId != enemyId)
			{
				continue;
			}

			bounty.AddProgress(1);
			EmitSignal(SignalName.OnPlayerBountyProgressUpdated, bounty.Id);

			if (bounty.CurrentProgress >= bounty.MaxProgress)
			{
				finishedBounties.Add(bounty);
			}
		}

		if (finishedBounties.Count > 0)
		{
			foreach (var bounty in finishedBounties)
			{
				// Remove finished bounties from player
				PlayerBounties.Remove(bounty);
				EmitSignal(SignalName.OnPlayerBountyRemoved, bounty.Id);

				// Display finished bounty notifications
				var notificationData = new NotificationData(bounty.Name, "Completed!");
				NotificationsManager.Instance.EnqueueNotification(notificationData);

				// Apply rewards to player
				if (bounty.ReputationReward != null)
				{
					PlayerManager.Instance.AddReputation((int)bounty.ReputationReward);
				}
				if (bounty.CurrencyReward != null)
				{
					PlayerManager.Instance.AddCurrency((int)bounty.CurrencyReward);
				}
				if (bounty.ResourceReward != null)
				{
					PlayerManager.Instance.AddResource((ResourceBundle)bounty.ResourceReward);
				}
			}
		}
	}

	public void DecrementBountyLifetimes()
	{
		foreach (var bounty in PlayerBounties)
		{
			bounty.DecrementLifetime();
			if (bounty.LifeTime <= 0)
			{
				RemovePlayerBounty(bounty);
			}
		}
	}
}