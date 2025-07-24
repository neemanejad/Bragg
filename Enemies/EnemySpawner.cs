using Godot;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene { get; set; }
	[Export]
	public double SpawnTimer { get; set; } = 5.0f;
	private Timer timer {  get; set; }

	private static readonly int BUFFER = 750;

	public override void _Ready()
	{
		timer = GetNode<Timer>("EnemyTimer");
		timer.WaitTime = SpawnTimer;
	}

	private void SpawnEnemy()
	{
		if (PlayerManager.PlayerNode == null)
		{
			return;
		}

		var enemyNode = EnemyScene.Instantiate<Enemy>();

		enemyNode.Position = CreateRandomPosition();

		AddChild(enemyNode);
	}

	private Vector2 CreateRandomPosition()
	{
		var screenSize = GetViewport().GetVisibleRect().Size;

		var randX = GetRandomScreenSizeRange(screenSize.X);
		var randY = GetRandomScreenSizeRange(screenSize.Y);

		var enemyPositionX = (float)(PlayerManager.PlayerNode.Position.X + randX);
		var enemyPositionY = (float)(PlayerManager.PlayerNode.Position.Y + randY);

		return new Vector2(enemyPositionX, enemyPositionY);
	}

	private static float GetRandomScreenSizeRange(float size)
	{
		// 1 or -1
		var multiplier = GD.Randf() > 0.5f ? 1 : -1;

		var positionWithBuffer = GD.RandRange(0, size) + BUFFER;

		return (float)(positionWithBuffer * multiplier);
	}
}
