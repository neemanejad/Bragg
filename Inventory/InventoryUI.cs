using System;
using System.Collections.Generic;
using System.Reflection;

using Godot;

using Redwood.Items;
using Redwood.UI;

public partial class InventoryUI : UI
{

	ItemList ItemList { get; set; }
	CanvasLayer CanvasLayer { get; set; }
	Dictionary<ItemId, int> Labels { get; set; } = new Dictionary<ItemId, int>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemList = GetNode<ItemList>("CanvasLayer/ItemList");
		CanvasLayer = GetNode<CanvasLayer>("CanvasLayer");

		foreach (var entry in PlayerManager.Instance.ItemCountMap)
		{
			var text = $"{Item.GetItem(entry.Key)} ({entry.Value})";
			var index = ItemList.AddItem(text);
			Labels.Add(entry.Key, index);
		}

		PlayerManager.Instance.ItemAdded += AddItem;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessKeyboardInput();
	}

	private void ProcessKeyboardInput()
	{
		if (Input.IsActionJustPressed("inventory") || Input.IsActionJustPressed("escape"))
		{
			QueueFree();
		}
	}

	private void AddItem(int itemId)
	{
		int label;
		Item item = Item.GetItem((ItemId)itemId);
		if (!Labels.TryGetValue(item.ID, out label))
		{
			label = ItemList.AddItem("");
			Labels.Add(item.ID, label);
		}

		ItemList.SetItemText(label, $"{item.Name} ({PlayerManager.Instance.ItemCountMap[item.ID]})");
	}

	public override void _ExitTree()
	{
		PlayerManager.Instance.ItemAdded -= AddItem;
		base._ExitTree();
	}
}