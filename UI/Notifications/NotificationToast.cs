using System;

using Godot;

using Redwood.Notifications;

public partial class NotificationToast : Control
{
	public NotificationData NotificationData { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("Panel/Title").Text = NotificationData.Title;
		GetNode<Label>("Panel/Description").Text = NotificationData.Description;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}