using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redwood.Notifications;
public partial class NotificationData
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }

	public NotificationData(string title, string description)
	{
		Id = Guid.NewGuid().ToString();
		Title = title;
		Description = description;
	}
}