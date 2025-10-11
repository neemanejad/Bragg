using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace Redwood.UI;

public partial class UI : Control
{
	[Export]
	public UIType UIType { get; set; }
	[Export]
	protected UISignalEmitter _uiSignalEmitter { get; set; }

	public override void _Ready()
	{

	}

	public override void _ExitTree()
	{
		_uiSignalEmitter.SignalRemoveUI(UIType);
	}
}