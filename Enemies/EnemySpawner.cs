using Godot;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene { get; set; }
	[Export]
	public double SpawnTimer { get; set; } = 5.0f;
	private Timer timer { get; set; }

	public override void _Ready()
	{
		timer = GetNode<Timer>("EnemyTimer");
		timer.WaitTime = SpawnTimer;
	}

	private void SpawnEnemy()
	{
		if (PlayerManager.Instance.PlayerNode == null)
		{
			return;
		}

		var enemyNode = EnemyScene.Instantiate<Enemy>();

		enemyNode.Position = LocationUtils.Instance.CreateRandomPosition();

		AddChild(enemyNode);
	}
}