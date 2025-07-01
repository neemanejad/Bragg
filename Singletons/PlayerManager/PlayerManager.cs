using Godot;
using Redwood.Items;
using System.Collections.Generic;

public partial class PlayerManager : Node
{
	[Signal]
	public delegate void ItemAddedEventHandler(int itemId);

	public Dictionary<ItemId, int> ItemCountMap  = new Dictionary<ItemId, int>();
	public static PlayerManager Instance { get; private set; }
	public static Player PlayerNode { get; private set; }

	private float _health = 100f;

	private int _killCount;

	public override void _Ready()
	{
		Instance = this;
	}

	public void OnKillHandler()
	{
		_killCount += 1;
		GD.Print(_killCount);
	}

	public void AddHealth(float health)
	{
		var healthSum = health + _health;

		if (healthSum > 100f)
		{
			_health = 100f;
			return;
		}
		else if (healthSum <= 0f)
		{
			_health = 0;
			return;
		}

		_health = healthSum;
	}

	public void AddItem(int itemId)
	{
		if (!ItemCountMap.TryGetValue((ItemId) itemId, out _))
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
}
