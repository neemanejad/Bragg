using Godot;
using System;

public partial class KillsLabelValue : Label
{
	public override void _Ready()
	{
		Text = PlayerManager.Instance.KillCount.ToString();
	}
}
