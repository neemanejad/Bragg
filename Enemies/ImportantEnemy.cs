using Godot;

public partial class ImportantEnemy : Enemy
{
	[Signal]
	public delegate void ImportantEnemyDefeatedEventHandler();

	public override void _Ready()
	{
		base._Ready();

		EnemyDefeated += ImportantEnemyDefeatedSignal;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		EnemyDefeated -= ImportantEnemyDefeatedSignal;
	}

	public void ImportantEnemyDefeatedSignal()
	{
		GameManager.Instance.LevelClearedSignal();
	}
}