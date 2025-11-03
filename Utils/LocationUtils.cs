using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using Redwood.Singletons;

public partial class LocationUtils : Node
{
	private int BUFFER = 750;

	public static LocationUtils Instance { get; private set; }

	private LocationUtils()
	{

	}

	public override void _Ready()
	{
		Instance = this;
	}

	public Vector2 CreateRandomPosition()
	{
		var screenSize = GetViewport().GetVisibleRect().Size;

		var randX = GetRandomScreenSizeRange(screenSize.X);
		var randY = GetRandomScreenSizeRange(screenSize.Y);

		var enemyPositionX = (float)(PlayerManager.Instance.PlayerNode.Position.X + randX);
		var enemyPositionY = (float)(PlayerManager.Instance.PlayerNode.Position.Y + randY);

		return new Vector2(enemyPositionX, enemyPositionY);
	}

	public float GetRandomScreenSizeRange(float size)
	{
		// 1 or -1
		var multiplier = GD.Randf() > 0.5f ? 1 : -1;

		var positionWithBuffer = GD.RandRange(0, size) + BUFFER;

		return (float)(positionWithBuffer * multiplier);
	}
}