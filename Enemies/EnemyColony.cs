using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;

using Redwood.Singletons.RandomUtils;

namespace Redwood.Enemies;

[Tool]
public partial class EnemyColony : Node2D
{
	[Export]
	public Array<PackedScene> Enemies;

	[Export]
	public Array<float> EnemyWeights;

	private int _enemyMinCount = 3;
	private int _enemyMaxCount = 7;

	public void Generate(Vector2I coords)
	{
		var enemyCount = RandomUtils.RNG.RandiRange(_enemyMinCount, _enemyMaxCount);
		List<Node2D> enemies = new();

		for (var i = 0; i < enemyCount; i++)
		{
			var randIndex = (int)RandomUtils.RNG.RandWeighted(EnemyWeights.ToArray());
			var enemy = Enemies[randIndex].Instantiate<Node2D>();
			enemies.Add(enemy);
		}

		foreach (var enemy in enemies)
		{
			enemy.GlobalPosition = new(RandomUtils.RNG.RandfRange(coords.X - 100, coords.X + 100), RandomUtils.RNG.RandfRange(coords.Y - 100, coords.Y + 100));
			AddChild(enemy);
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = new();

		if (Enemies.Count == 0 || EnemyWeights.Count == 0)
		{
			warnings.Add("Enemies and EnemyWeights arrays must be greater than 0.");
		}

		if (Enemies.Count != EnemyWeights.Count)
		{
			warnings.Add("Enemies and EnemyWeights must be the same length.");
		}

		return warnings.ToArray();
	}
}