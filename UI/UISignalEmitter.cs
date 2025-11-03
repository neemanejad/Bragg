using Godot;

using Redwood.Utils;

namespace Redwood.UI;

public partial class UISignalEmitter : Node
{
	[Signal]
	public delegate void AddUIEventHandler(int id);

	[Signal]
	public delegate void RemoveUIEventHandler(int id);

	UIManager UIManager { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UIManager = TreeUtils.GetRootUINode(this);
		Connect(SignalName.AddUI, new Callable(UIManager, UIManager.MethodName.AddUI));
		Connect(SignalName.RemoveUI, new Callable(UIManager, UIManager.MethodName.RemoveCurrentUI));
	}

	public void SignalAddUI(UIType ui)
	{
		EmitSignal(SignalName.AddUI, (int)ui);
	}

	public void SignalRemoveUI(UIType ui)
	{
		EmitSignal(SignalName.RemoveUI, (int)ui);
	}
}