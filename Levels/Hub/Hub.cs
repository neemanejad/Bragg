using System.Collections.Generic;

using Godot;

using Redwood.Scripts.Core;
using Redwood.Scripts.World;

public partial class Hub : Node2D
{
	private WorldManager _worldManager;

	private readonly List<World> _worlds = new();
	public override void _Ready()
	{
		_worldManager = GetNode<WorldManager>("WorldManager");

		for (var i = 0; i < 10; i++)
		{
			var world = _worldManager.GenerateWorld();
			_worlds.Add(world);
		}
	}
}