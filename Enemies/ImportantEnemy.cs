using Godot;

namespace Redwood.Enemies;

public partial class ImportantEnemy : Node2D
{
	[Signal]
	public delegate void ImportantEnemyDefeatedEventHandler();

	public override void _Ready()
	{
		var enemyNode = GetNode<Enemy>("Enemy");
		enemyNode.EnemyDefeated += ImportantEnemyDefeatedSignal;
	}

	public override void _ExitTree()
	{
		var enemyNode = GetNode<Enemy>("Enemy");
		enemyNode.EnemyDefeated -= ImportantEnemyDefeatedSignal;
	}

	public void ImportantEnemyDefeatedSignal(string enemyId)
	{
		GameManager.Instance.LevelClearedSignal();
	}
}