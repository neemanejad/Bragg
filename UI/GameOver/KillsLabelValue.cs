using System;

using Godot;

public partial class KillsLabelValue : Label
{
	public override void _Ready()
	{
		Text = PlayerManager.Instance.KillCount.ToString();
	}
}