using System;

using Godot;

using Redwood.UI;
using Redwood.Utils;

public partial class UISignalEmitter : Node
{
	[Signal]
	public delegate void AddUIEventHandler(int id);
	[Signal]
	public delegate void RemoveUIEventHandler(int id);
	UIManager _uiManager { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_uiManager = TreeUtils.GetRootUINode(this);
		Connect(SignalName.AddUI, new Callable(_uiManager, UIManager.MethodName.AddUI));
		Connect(SignalName.RemoveUI, new Callable(_uiManager, UIManager.MethodName.RemoveCurrentUI));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

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