using System;
using System.Collections;
using System.Collections.Generic;

using Godot;

using Redwood.Notifications;
using Redwood.Singletons.NotificationsManager;

public partial class NotificationUI : Control
{
	Timer _notificationVisibilityTimer;
	readonly Queue<NotificationToast> _notificationQueue = new();
	VBoxContainer _notificationContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NotificationsManager.Instance.Connect(NotificationsManager.SignalName.OnNotificationAdded, new Callable(this, MethodName.AddNotificationToQueue));

		_notificationVisibilityTimer = GetNode<Timer>("NotificationVisibilityTimer");
		_notificationVisibilityTimer.Connect(Timer.SignalName.Timeout, new Callable(this, MethodName.NotificationVisibilityTimeout));

		_notificationContainer = GetNode<VBoxContainer>("CanvasLayer/HBoxContainer/NotificationContainer");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddNotificationToQueue()
	{
		var notificationData = NotificationsManager.Instance.GetLatestNotification();
		var notificationToast = ResourceLoader.Load<PackedScene>("res://UI/Notifications/NotificationToast.tscn").Instantiate<NotificationToast>();
		notificationToast.NotificationData = notificationData;
		_notificationQueue.Enqueue(notificationToast);

		if (_notificationVisibilityTimer.IsStopped())
		{
			_notificationContainer.AddChild(notificationToast);
			_notificationVisibilityTimer.Start();
		}
	}

	public void NotificationVisibilityTimeout()
	{
		var currentlyShownNotificaiton = _notificationQueue.Dequeue();
		currentlyShownNotificaiton.QueueFree();

		if (_notificationQueue.Count > 0)
		{
			_notificationContainer.AddChild(_notificationQueue.Peek());
			_notificationVisibilityTimer.Start();
		}
	}
}