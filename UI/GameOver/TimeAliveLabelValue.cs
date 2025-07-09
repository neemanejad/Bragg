using Godot;
using System;

public partial class TimeAliveLabelValue : Label
{
	public override void _Ready()
	{
		Text = GameManager.Instance.ElapsedTime.ToString();
	}
}
