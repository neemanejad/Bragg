using System;
using System.Collections.Generic;

using Godot;

using Redwood.Items;

public partial class PlayerManager : Node
{
	[Signal]
	public delegate void ItemAddedEventHandler(int itemId);

	public Dictionary<ItemId, int> ItemCountMap = new Dictionary<ItemId, int>();

	public static PlayerManager Instance { get; private set; }

	public static Player PlayerNode { get; private set; }
	public static BaseWeapon WeaponNode { get; private set; }

	private float _health = 100f;

	public float Health
	{
		get { return _health; }
	}

	private int _killCount = 0;

	public int KillCount { get { return _killCount; } }

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

		var healthComponent = PlayerNode.GetNode<HealthComponent>("Vitals/UI/HealthComponent");
		healthComponent.RemoveHealth(health);
	}

	public void SubtractHealth(float health)
	{
		var healthComponent = PlayerNode.GetNode<HealthComponent>("Vitals/UI/HealthComponent");

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

	public static void ClearPlayerNode()
	{
		PlayerNode = null;
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
		WeaponNode.StartShooting();
	}

	internal void StopShooting()
	{
		WeaponNode.StopShooting();
	}
}