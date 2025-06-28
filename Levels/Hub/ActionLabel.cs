using Godot;
using System;

public partial class ActionLabel : CanvasLayer
{
	public override void _Ready()
	{
		Hide();
	}

	private void HideLabel(Node2D body)
	{
		Hide();
	}

	private void ShowLabel(Node2D body)
	{
		Show();
	}
}
