using System;
using System.Collections.Generic;

using Godot;

using Redwood.Items;
using Redwood.PlayerEntity;
using Redwood.Resources;
using Redwood.Weapons;

namespace Redwood.Singletons;

public partial class PlayerManager : Node
{
	[Signal]
	public delegate void ItemAddedEventHandler(int itemId);
	public Dictionary<ItemId, int> ItemCountMap { get; private set; } = new Dictionary<ItemId, int>();
	public static PlayerManager Instance { get; private set; }
	public Player PlayerNode { get; private set; }
	public BaseWeapon WeaponNode { get; private set; }
	public float Health
	{
		get { return _health; }
	}

	private int _killCount = 0;
	private float _health = 100f;

	public int KillCount { get { return _killCount; } }
	public int ReputationCount { get; private set; }
	public int CurrencyCount { get; private set; }
	public Dictionary<ResourceId, ResourceBundle> ResourceCount { get; private set; }

	public void AddReputation(int reputation)
	{
		ReputationCount += reputation;
	}

	public void RemoveReputation(int reputation)
	{
		ReputationCount -= reputation;
	}

	public void AddCurrency(int currency)
	{
		CurrencyCount += currency;
	}

	public void RemoveCurrency(int currency)
	{
		CurrencyCount -= currency;
	}

	public void AddResource(ResourceBundle resource)
	{
		if (!ResourceCount.ContainsKey(resource.Resource.Id))
		{
			ResourceCount[resource.Resource.Id] = resource;
		}
		else
		{
			ResourceCount[resource.Resource.Id].Quantity += resource.Quantity;
		}
	}

	public void RemoveResource(ResourceBundle resource)
	{
		if (!ResourceCount.ContainsKey(resource.Resource.Id))
		{
			return;
		}

		var resourceBundle = ResourceCount[resource.Resource.Id];
		resourceBundle.Quantity -= resource.Quantity;
		if (resourceBundle.Quantity < 0)
		{
			resourceBundle.Quantity = 0;
		}
	}

	public override void _Ready()
	{
		Instance = this;
	}

	public void OnKillHandler()
	{
		_killCount += 1;
	}

	public void Reset()
	{
		_health = 100f;
		var healthComponent = PlayerNode.GetNode<HealthComponent>("Vitals/HealthComponent");
		healthComponent.SetHealth(_health);
	}

	public void AddHealth(float health)
	{
		if (health < 0)
		{
			throw new Exception("health should be >= 0");
		}

		var healthSum = health + _health;

		if (healthSum > 100f)
		{
			_health = 100f;
			return;
		}

		_health = healthSum;

		var healthComponent = PlayerNode.GetNode<HealthComponent>("Vitals/HealthComponent");
		healthComponent.RemoveHealth(health);
	}

	public void SubtractHealth(float health)
	{
		var healthComponent = PlayerNode.GetNode<HealthComponent>("Vitals/HealthComponent");

		if (health < 0)
		{
			throw new Exception("health should be >= 0");
		}

		var healthDiff = _health - health;

		if (healthDiff <= 0)
		{
			_health = 0;

			GameManager.Instance.End();

			return;
		}

		_health = healthDiff;

		healthComponent.RemoveHealth(health);

	}

	public void AddItem(int itemId)
	{
		if (!ItemCountMap.TryGetValue((ItemId)itemId, out _))
		{
			ItemCountMap[(ItemId)itemId] = 0;
		}

		ItemCountMap[(ItemId)itemId] += 1;
		Item.GetItem((ItemId)itemId).Effect(PlayerNode);
		EmitSignal(SignalName.ItemAdded, itemId);
	}

	public void SetPlayerNode(Player player)
	{
		PlayerNode = player;
	}

	public void ShowSurvivalTimer()
	{
		var canvasLayer = PlayerNode.GetNode<CanvasLayer>("SurvivalTimer/CanvasLayer");
		canvasLayer.Visible = true;
	}

	public void HideSurvivalTimer()
	{
		var canvasLayer = PlayerNode.GetNode<CanvasLayer>("SurvivalTimer/CanvasLayer");
		canvasLayer.Visible = false;
	}

	internal void SetWeapon(BaseWeapon baseWeapon)
	{
		WeaponNode = baseWeapon;
	}

	internal void StartShooting()
	{
		if (WeaponNode == null)
		{
			return;
		}

		WeaponNode.StartShooting();
	}

	internal void StopShooting()
	{
		if (WeaponNode == null)
		{
			return;
		}

		WeaponNode.StopShooting();
	}
}