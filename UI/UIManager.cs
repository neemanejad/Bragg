using Godot;
using Godot.Collections;

namespace Redwood.UI;

public partial class UIManager : Control
{
	private UIControl _displayedUi = null;

	[Export]
	public Array<PackedScene> Menus { get; set; }

	readonly Dictionary<UIType, PackedScene> _uis = new();

	public override void _Ready()
	{
		foreach (var menuPackedScene in Menus)
		{
			var menu = menuPackedScene.Instantiate<UIControl>();
			_uis[menu.UIType] = menuPackedScene;
		}
	}

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
		var ui = _uis[type].Instantiate<UIControl>();
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