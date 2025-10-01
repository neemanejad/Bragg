using Godot;
using Project.Utils;
using System;

public partial class TimeAliveLabelValue : Label
{
	public override void _Ready()
	{
        Text = TimeUtils.FormatSecondsToSurvivalTime(GameManager.Instance.SurvivalTime);
	}
}
