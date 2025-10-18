using Godot;

public partial class ImportantEnemy : Enemy
{
	[Signal]
	public delegate void ImportantEnemyDefeatedEventHandler();

	public override void _Ready()
	{
		Connect(SignalName.ReceivedDamage, new Callable(GetNode<DamageIndicator>("DamageIndicator"), DamageIndicator.MethodName.ShowDamage));

		base._Ready();

		EnemyDefeated += ImportantEnemyDefeatedSignal;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		EnemyDefeated -= ImportantEnemyDefeatedSignal;
	}

	public void ImportantEnemyDefeatedSignal(string enemyId)
	{
		GameManager.Instance.LevelClearedSignal();
	}
}