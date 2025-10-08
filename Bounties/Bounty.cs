using System;

using Redwood.Resources;

namespace Redwood.Bounties;

public enum BountyType
{
	ELIMINATION,
	COLLECTION
}

public interface Bounty
{
	public string Id { get; }
	public String Name { get; }
	public String Description { get; }
	public String TargetId { get; }
	public BountyType Type { get; }
	public int? ReputationReward { get; }
	public int? CurrencyReward { get; }
	public bool IsTimeBased { get; }
	public int? BountyTimeLimitSeconds { get; }
	public ResourceBundle? ResourceReward { get; }
	/// <summary>
	/// The number of deployments the bounty stays open.
	/// </summary>
	public int? LifeTime { get; }
	public int CurrentProgress { get; }
	public int MaxProgress { get; }
	public void AddProgress(int progress);
	public void DecrementLifetime();
}

internal class EliminationBounty : Bounty
{
	public EliminationBounty(String Name, String Description, String TargetId, int ReputationReward, int CurrencyReward, bool IsTimeBased, int? BountyTimeLimitSeconds, ResourceBundle? ResourceReward, int? LifeTime, int MaxProgress)
	{
		this.Id = Guid.NewGuid().ToString();
		this.Name = Name;
		this.Description = Description;
		this.TargetId = TargetId;
		this.ReputationReward = ReputationReward;
		this.CurrencyReward = CurrencyReward;
		this.ResourceReward = ResourceReward;
		this.IsTimeBased = IsTimeBased;
		this.BountyTimeLimitSeconds = BountyTimeLimitSeconds;
		this.LifeTime = LifeTime;
		this.CurrentProgress = 0;
		this.MaxProgress = MaxProgress;
	}

	public string Id { get; private set; }

	public BountyType Type { get; private set; } = BountyType.ELIMINATION;

	public string Name { get; private set; }

	public string Description { get; private set; }
	public string TargetId { get; private set; }

	public int? ReputationReward { get; private set; }

	public int? CurrencyReward { get; private set; }

	public bool IsTimeBased { get; private set; }

	public int? BountyTimeLimitSeconds { get; private set; }

	public ResourceBundle? ResourceReward { get; private set; }

	public int? LifeTime { get; private set; }

	public int CurrentProgress { get; private set; }
	public int MaxProgress { get; private set; }
	public void AddProgress(int progress)
	{
		CurrentProgress += progress;
	}
	public void DecrementLifetime()
	{
		if (LifeTime <= 0) return;

		LifeTime -= 0;
	}
}

internal class CollectionBounty : Bounty
{
	public CollectionBounty(String Name, String Description, String TargetId, int ReputationReward, int CurrencyReward, bool IsTimeBased, int? BountyTimeLimitSeconds, ResourceBundle? ResourceReward, int? LifeTime, int MaxProgress)
	{
		this.Id = Guid.NewGuid().ToString();
		this.Name = Name;
		this.Description = Description;
		this.TargetId = TargetId;
		this.ReputationReward = ReputationReward;
		this.CurrencyReward = CurrencyReward;
		this.ResourceReward = ResourceReward;
		this.IsTimeBased = IsTimeBased;
		this.BountyTimeLimitSeconds = BountyTimeLimitSeconds;
		this.LifeTime = LifeTime;
		this.CurrentProgress = 0;
		this.MaxProgress = MaxProgress;
	}

	public string Id { get; private set; }
	public BountyType Type { get; private set; } = BountyType.COLLECTION;

	public string Name { get; private set; }

	public string Description { get; private set; }
	public string TargetId { get; private set; }

	public int? ReputationReward { get; private set; }

	public int? CurrencyReward { get; private set; }

	public bool IsTimeBased { get; private set; }

	public int? BountyTimeLimitSeconds { get; private set; }

	public ResourceBundle? ResourceReward { get; private set; }

	public int? LifeTime { get; private set; }

	public int CurrentProgress { get; private set; }
	public int MaxProgress { get; private set; }

	public void AddProgress(int progress)
	{
		CurrentProgress += progress;
	}
	public void DecrementLifetime()
	{
		if (LifeTime <= 0) return;

		LifeTime -= 0;
	}
}