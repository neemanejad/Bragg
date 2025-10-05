using System.Collections.Generic;

using Godot;

using Redwood.Scripts.Core;
using Redwood.Scripts.World;

namespace Redwood.Singletons.WorldManager;

public partial class WorldManager : Node
{
	public static WorldManager Instance { get; private set; }

	private readonly WorldGenerator _worldGenerator = new();

	public List<WorldData> Worlds { get; private set; }

	public WorldData SelectedWorld { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void Generate(int worldCount)
	{
		List<WorldData> worlds = new();

		for (var i = 0; i < worldCount; i++)
		{
			worlds.Add(_worldGenerator.GenerateWorld());
		}

		Worlds = worlds;
	}

	public void Select(WorldData world)
	{
		SelectedWorld = world;
	}
}