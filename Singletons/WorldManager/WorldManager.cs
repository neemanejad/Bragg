namespace Singletons.WorldManager;

using System.Collections.Generic;

using Godot;

using Redwood.Scripts.Core;
using Redwood.Scripts.World;

public partial class WorldManager : Node
{
	public static WorldManager Instance { get; private set; }

	private readonly WorldGenerator _worldGenerator = new();

	public List<World> Worlds { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void Generate(int worldCount)
	{
		List<World> worlds = new();

		for (var i = 0; i < worldCount; i++)
		{
			worlds.Add(_worldGenerator.GenerateWorld());
		}

		Worlds = worlds;
	}
}