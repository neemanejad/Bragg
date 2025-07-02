using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene { get; set; }

	public override void _Ready()
	{
	}

	private void SpawnEnemy()
	{
		var screenSize = GetViewport().GetVisibleRect().Size;

		var randX = GD.RandRange(-screenSize.X, screenSize.X);
		var randY = GD.RandRange(-screenSize.Y, screenSize.Y);

		var enemyPositionX = (float)(PlayerManager.PlayerNode.Position.X + randX);
		var enemyPositionY = (float)(PlayerManager.PlayerNode.Position.Y + randY);


		var enemyNode = EnemyScene.Instantiate<Enemy>();
		enemyNode.Position = new Vector2(enemyPositionX, enemyPositionY);

		AddChild(enemyNode);
	}
}
