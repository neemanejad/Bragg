using System;
using System.Collections.Generic;

using Godot;
using Godot.Collections;

using Redwood.Menus;

namespace Redwood.UI;

public partial class UIManager : Control
{
	UI _displayedUi = null;
	[Export]
	public Array<PackedScene> Menus { get; set; }
	readonly System.Collections.Generic.Dictionary<UIType, PackedScene> _uis = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var menuPackedScene in Menus)
		{
			var menu = menuPackedScene.Instantiate<UI>();
			_uis[menu.UIType] = menuPackedScene;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventory"))
		{
			AddUI((int)UIType.InventoryMenu);
		}

		if (Input.IsActionJustPressed("escape"))
		{
			AddUI((int)UIType.PauseMenu);
		}

		if (Input.IsActionJustPressed("bounties"))
		{
			AddUI((int)UIType.PlayerBountyMenu);
		}
	}

	public void AddUI(int id)
	{
		if (_displayedUi != null)
		{
			return;
		}

		UIType type = (UIType)id;
		var ui = _uis[type].Instantiate<UI>();
		_displayedUi = ui;
		AddChild(ui);
	}

	public void RemoveCurrentUI(int id)
	{
		UIType type = (UIType)id;
		if (type != _displayedUi.UIType)
		{
			return;
		}

		_displayedUi = null;
		GetTree().Paused = false;
	}
}