using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using Redwood.Notifications;

namespace Redwood.Singletons.NotificationsManager;
public partial class NotificationsManager : Node
{
	[Signal]
	public delegate void OnNotificationAddedEventHandler();

	readonly Queue<NotificationData> _notificationsQueue = new();
	public static NotificationsManager Instance { get; set; }
	public override void _Ready()
	{
		Instance = this;
	}

	public NotificationData GetLatestNotification()
	{
		return _notificationsQueue.Dequeue();
	}

	public void EnqueueNotification(NotificationData notification)
	{
		_notificationsQueue.Enqueue(notification);
		EmitSignal(SignalName.OnNotificationAdded);
	}
}