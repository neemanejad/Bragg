using Godot;

namespace Redwood.UI;

public partial class UIControl : Control
{
	[Export]
	public UIType UIType { get; set; }
	[Export]
	protected UISignalEmitter UiSignalEmitter { get; set; }

	public override void _Ready()
	{

	}

	public override void _ExitTree()
	{
		UiSignalEmitter.SignalRemoveUI(UIType);
	}
}